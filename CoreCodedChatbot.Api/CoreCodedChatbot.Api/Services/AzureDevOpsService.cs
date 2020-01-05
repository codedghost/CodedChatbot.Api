using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreCodedChatbot.Api.Interfaces.Commands;
using CoreCodedChatbot.Api.Interfaces.Services;
using CoreCodedChatbot.ApiContract.ResponseModels.DevOps.ChildModels;
using CoreCodedChatbot.Config;
using CoreCodedChatbot.Secrets;
using Microsoft.Extensions.Logging;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using Microsoft.VisualStudio.Services.WebApi.Patch.Json;

namespace CoreCodedChatbot.Api.Services
{
    public class AzureDevOpsService : IAzureDevOpsService
    {
        private readonly IConfigService _configService;
        private readonly ICreateJsonPatchDocumentFromBugRequestCommand _createJsonPatchDocumentFromBugRequestCommand;
        private readonly ILogger<AzureDevOpsService> _logger;
        private readonly WorkItemTrackingHttpClient _workItemTrackingClient;

        public AzureDevOpsService(
            ISecretService secretService,
            IConfigService configService,
            ICreateJsonPatchDocumentFromBugRequestCommand createJsonPatchDocumentFromBugRequestCommand,
            ILogger<AzureDevOpsService> logger
        )
        {
            _configService = configService;
            _createJsonPatchDocumentFromBugRequestCommand = createJsonPatchDocumentFromBugRequestCommand;
            _logger = logger;
            var vssConnection = new VssConnection(
                new Uri(secretService.GetSecret<string>("DevOpsChatbotCollectionUrl")),
                new VssBasicCredential(string.Empty, secretService.GetSecret<string>("DevOpsChatbotPAT")));

            _workItemTrackingClient = vssConnection.GetClient<WorkItemTrackingHttpClient>();
        }

        public async Task<List<WorkItem>> GetCommittedPbisForThisIteration()
        {
            var currentIterationPbisQueryId = Guid.Parse(_configService.Get<string>("DevOpsCurrentPBIsQueryId"));

            List<int> pbiIds;

            try
            {
                var pbiQueryResult = await _workItemTrackingClient.QueryByIdAsync(currentIterationPbisQueryId);

                if (pbiQueryResult == null || !pbiQueryResult.WorkItems.Any())
                    return new List<WorkItem>();

                pbiIds = pbiQueryResult.WorkItems.Select(wi => wi.Id).ToList();
            }
            catch (VssServiceException e)
            {
                _logger.LogError(e, $"Could not load the Current Iteration PBI Query: {currentIterationPbisQueryId}");
                return null;
            }

            try
            {
                var pbiQuery = await _workItemTrackingClient.GetWorkItemsAsync(pbiIds, expand: WorkItemExpand.Relations);

                return pbiQuery ?? new List<WorkItem>();
            }
            catch (VssServiceException e)
            {
                _logger.LogError(e, "Could not get the PBIs found by the Query");
                return null;
            }
        }

        public async Task<List<WorkItem>> GetChildWorkItemsByPbi(WorkItem pbi)
        {
            if (!pbi?.Relations?.Any(r =>
                string.Equals((string) r.Attributes["name"], "child", StringComparison.InvariantCultureIgnoreCase)) ?? true)
                return new List<WorkItem>();

            try
            {
                var workItemIds = pbi.Relations
                    .Where(r => string.Equals((string) r.Attributes["name"], "child",
                        StringComparison.InvariantCultureIgnoreCase))
                    .Select(child => string.IsNullOrWhiteSpace(child.Url)
                        ? 0
                        : int.Parse(child.Url.Split('/').Last()));

                var workItems = await _workItemTrackingClient.GetWorkItemsAsync(workItemIds);

                return workItems;
            }
            catch (VssServiceException e)
            {
                _logger.LogError(e, $"Could not retrieve the work items assigned to pbi: {pbi.Id}");
                return new List<WorkItem>();
            }
            catch (Exception e) when
            (e is ArgumentNullException ||
             e is ArgumentException)
            {
                _logger.LogError(e, $"Could not parse a child URL from pbi: {pbi.Id}");
                return new List<WorkItem>();
            }
        }

        public async Task<WorkItem> GetWorkItemById(int id)
        {
            var workItem = await _workItemTrackingClient.GetWorkItemAsync(id);
            return workItem;
        }

        public async Task<bool> RaiseBugInBacklog(string twitchUsername, DevOpsBug bugInfo)
        {
            try
            {
                var jsonPatch = _createJsonPatchDocumentFromBugRequestCommand.Create(twitchUsername, bugInfo);

                await _workItemTrackingClient.CreateWorkItemAsync(jsonPatch, _configService.Get<string>("DevOpsProjectName"), "Bug");

                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e,
                    $"Could not raise bug: {twitchUsername}, {bugInfo.Title}, {bugInfo.ReproSteps}, {bugInfo.AcceptanceCriteria}, {bugInfo.SystemInfo}");
                return false;
            }
        }
    }
}