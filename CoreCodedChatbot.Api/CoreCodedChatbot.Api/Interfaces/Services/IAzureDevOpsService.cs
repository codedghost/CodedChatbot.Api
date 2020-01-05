using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;

namespace CoreCodedChatbot.Api.Interfaces.Services
{
    public interface IAzureDevOpsService
    {
        Task<WorkItem> GetWorkItemById(int id);
        Task<List<WorkItem>> GetCommittedPbisForThisIteration();
        Task<List<WorkItem>> GetChildWorkItemsByPbi(WorkItem pbi);
    }
}