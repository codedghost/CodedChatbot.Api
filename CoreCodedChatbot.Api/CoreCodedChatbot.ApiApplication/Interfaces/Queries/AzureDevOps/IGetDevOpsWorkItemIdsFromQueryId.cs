using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Queries.AzureDevOps
{
    public interface IGetDevOpsWorkItemIdsFromQueryId
    {
        Task<List<int>> Get(WorkItemTrackingHttpClient client, Guid queryId);
    }
}