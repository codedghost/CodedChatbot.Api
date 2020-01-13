using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.VisualStudio.Services.Common;

namespace CoreCodedChatbot.Api.Queries
{
    public class GetDevOpsWorkItemIdsFromQueryId : IGetDevOpsWorkItemIdsFromQueryId
    {
        private readonly ILogger<GetDevOpsWorkItemIdsFromQueryId> _logger;

        public GetDevOpsWorkItemIdsFromQueryId(ILogger<GetDevOpsWorkItemIdsFromQueryId> logger)
        {
            _logger = logger;
        }

        public async Task<List<int>> Get(WorkItemTrackingHttpClient client, Guid queryId)
        {
            try
            {
                var queryResult = await client.QueryByIdAsync(queryId);

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
    }
}