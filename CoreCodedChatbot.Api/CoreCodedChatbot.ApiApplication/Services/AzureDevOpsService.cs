using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Extensions;
using CoreCodedChatbot.ApiApplication.Interfaces.Services;
using CoreCodedChatbot.ApiContract.ResponseModels.DevOps.ChildModels;
using CoreCodedChatbot.Config;
using CoreCodedChatbot.Secrets;
using Microsoft.Extensions.Logging;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using Microsoft.VisualStudio.Services.WebApi.Patch.Json;

namespace CoreCodedChatbot.ApiApplication.Services;

public class AzureDevOpsService : IBaseService, IAzureDevOpsService
{
    private readonly IConfigService _configService;
    private readonly ILogger<AzureDevOpsService> _logger;
    private readonly WorkItemTrackingHttpClient _workItemTrackingClient;
    //private readonly WorkHttpClient _workClient;

    public AzureDevOpsService(
        ISecretService secretService,
        IConfigService configService,
        ILogger<AzureDevOpsService> logger
    )
    {
        _configService = configService;
        _logger = logger;
        var vssConnection = new VssConnection(
            new Uri(secretService.GetSecret<string>("DevOpsChatbotCollectionUrl")),
            new VssBasicCredential(string.Empty, secretService.GetSecret<string>("DevOpsChatbotPAT")));

        _workItemTrackingClient = vssConnection.GetClient<WorkItemTrackingHttpClient>();

        //_workClient = vssConnection.GetClient<WorkHttpClient>();
    }

    public async Task<List<WorkItem>> GetCommittedPbisForThisIteration()
    {
        var currentIterationPbisQueryId = Guid.Parse(_configService.Get<string>("DevOpsCurrentPBIsQueryId"));

        var pbiIds = await GetItemIds(currentIterationPbisQueryId);

        if (pbiIds == null || !pbiIds.Any())
            return new List<WorkItem>();

        try
        {
            var pbiQuery = await _workItemTrackingClient.GetWorkItemsAsync(pbiIds, expand: WorkItemExpand.Relations);

            return pbiQuery ?? new List<WorkItem>();
        }
        catch (VssServiceException e)
        {
            _logger.LogError(e, "Could not get the PBIs found by the Current Iteration Query");
            return new List<WorkItem>();
        }
    }

    public async Task<List<WorkItem>> GetBacklogWorkItems()
    {
        var backlogItemsQueryId = Guid.Parse(_configService.Get<string>("DevOpsBacklogPBIsQueryId"));

        var itemIds = await GetItemIds(backlogItemsQueryId);

        if (itemIds == null || !itemIds.Any())
            return new List<WorkItem>();

        try
        {
            var workItems =
                await _workItemTrackingClient.GetWorkItemsAsync(itemIds, expand: WorkItemExpand.Relations);

            return workItems ?? new List<WorkItem>();
        }
        catch (VssServiceException e)
        {
            _logger.LogError(e, "Could not get the PBIs found by the Backlog Query");
            return new List<WorkItem>();
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
            var jsonPatchDocument = BuildJsonDocForWorkItem(twitchUsername, bugInfo);

            jsonPatchDocument
                .AddReproSteps(bugInfo.ReproSteps)
                .AddSystemInfo(bugInfo.SystemInfo);

            await _workItemTrackingClient.CreateWorkItemAsync(jsonPatchDocument,
                _configService.Get<string>("DevOpsProjectName"), "Bug");

            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e,
                $"Could not raise bug: {twitchUsername}, {bugInfo.Title}, {bugInfo.ReproSteps}, {bugInfo.AcceptanceCriteria}, {bugInfo.SystemInfo}");
            return false;
        }
    }

    public async Task RaisePracticeSongRequest(string twitchUsername, DevOpsProductBacklogItem songRequest)
    {
        var jsonPatchDocument = BuildJsonDocForWorkItem(twitchUsername, songRequest);

        jsonPatchDocument
            .AddDescription(songRequest.Description);

        await _workItemTrackingClient.CreateWorkItemAsync(jsonPatchDocument,
            _configService.Get<string>("DevOpsProjectName"), "Product Backlog Item");
    }

    private JsonPatchDocument BuildJsonDocForWorkItem(string twitchUsername, DevOpsWorkItem workItem)
    {
        var jsonPatchDocument = new JsonPatchDocument();
        
        jsonPatchDocument
        .AddTitle($"{twitchUsername} - {workItem.Title}")
            .AddAcceptanceCriteria(workItem.AcceptanceCriteria);
        if (workItem.Tags != null && workItem.Tags.Any())
            jsonPatchDocument.AddTags(workItem.Tags);

        return jsonPatchDocument;
    }

    private async Task<List<int>?> GetItemIds(Guid queryId)
    {
        try
        {
            var queryResult = await _workItemTrackingClient.QueryByIdAsync(queryId);

            if (queryResult == null || !queryResult.WorkItems.Any())
                return new List<int>();

            return queryResult.WorkItems.Select(wi => wi.Id).ToList();
        }
        catch (VssServiceException e)
        {
            _logger.LogError(e, $"Could not load Dev Ops Query: {queryId}");
            return null;
        }
    }

    //public async Task<string> GetCurrentSprintBurndownChart()
    //{
    //    try
    //    {
    //        var projectName = _configService.GetCommandText<string>("DevOpsProjectName");
    //        var iterations = await _workClient.GetTeamIterationsAsync(new TeamContext(projectName), "Current");
    //        var boardImage = await _workClient.GetIterationChartImageAsync(
    //            new TeamContext(projectName, "CodedChatbot Team"), iterations.First().Id,
    //            "Burndown");

    //        return string.Empty;
    //    }
    //    catch (Exception e)
    //    {
    //        throw;
    //    }


    //}
}