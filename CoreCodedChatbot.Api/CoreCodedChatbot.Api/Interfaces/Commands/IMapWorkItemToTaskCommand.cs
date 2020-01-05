using CoreCodedChatbot.ApiContract.ResponseModels.DevOps.ChildModels;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;

namespace CoreCodedChatbot.Api.Interfaces.Commands
{
    public interface IMapWorkItemToTaskCommand
    {
        DevOpsTask Map(WorkItem childWorkItem);
    }
}