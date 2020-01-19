using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;

namespace CoreCodedChatbot.Api.Interfaces.Queries
{
    public interface IGetDevOpsWorkItemIdsFromQueryId
    {
        Task<List<int>> Get(WorkItemTrackingHttpClient client, Guid queryId);
    }
}