using System.Collections.Generic;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiContract.ResponseModels.DevOps.ChildModels;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Services;

public interface IAzureDevOpsService
{
    Task<WorkItem> GetWorkItemById(int id);
    Task<List<WorkItem>> GetCommittedPbisForThisIteration();
    Task<List<WorkItem>> GetChildWorkItemsByPbi(WorkItem pbi);
    Task<bool> RaiseBugInBacklog(string twitchUsername, DevOpsBug bugInfo);
    Task<List<WorkItem>> GetBacklogWorkItems();
    Task RaisePracticeSongRequest(string twitchUsername, DevOpsProductBacklogItem songRequest);
    //Task<string> GetCurrentSprintBurndownChart();
}