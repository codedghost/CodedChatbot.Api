using CoreCodedChatbot.Api.Extensions;
using CoreCodedChatbot.Api.Interfaces.Commands.AzureDevOps;
using CoreCodedChatbot.ApiContract.ResponseModels.DevOps.ChildModels;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;

namespace CoreCodedChatbot.Api.Commands.AzureDevOps
{
    public class MapWorkItemToTaskCommand : IMapWorkItemToTaskCommand
    {
        public DevOpsTask Map(WorkItem childWorkItem)
        {
            if (childWorkItem.WorkItemType() != "Task")
                return null;

            return new DevOpsTask
            {
                Id = childWorkItem.Id ?? 0,
                Title = childWorkItem.Title(),
                Description = childWorkItem.Description(),
                AssignedTo = childWorkItem.AssignedTo(),
                State = childWorkItem.State(),
                RemainingWork = childWorkItem.RemainingWork()
            };
        }
    }
}