using CoreCodedChatbot.ApiContract.ResponseModels.DevOps.ChildModels;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;

namespace CoreCodedChatbot.Api.Interfaces.Commands
{
    public interface IMapWorkItemToParentWorkItemCommand
    {
        DevOpsWorkItem Map(WorkItem parentWorkItem);
    }
}