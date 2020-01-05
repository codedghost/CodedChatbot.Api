using CoreCodedChatbot.Api.Extensions;
using CoreCodedChatbot.Api.Interfaces.Commands;
using CoreCodedChatbot.ApiContract.ResponseModels.DevOps.ChildModels;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;

namespace CoreCodedChatbot.Api.Commands
{
    public class MapWorkItemToTaskCommand : IMapWorkItemToTaskCommand
    {
        public DevOpsTask Map(WorkItem childWorkItem)
        {
            if (childWorkItem.WorkItemType() != "Task")
                return null;

            return new DevOpsTask
            {
                Title = childWorkItem.Title(),
                Description = childWorkItem.Description(),
                AssignedTo = childWorkItem.AssignedTo(),
                State = childWorkItem.State(),
                RemainingWork = childWorkItem.RemainingWork()
            };
        }
    }
}