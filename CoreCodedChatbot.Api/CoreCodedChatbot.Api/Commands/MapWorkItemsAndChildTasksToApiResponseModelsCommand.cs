using System.Collections.Generic;
using System.Linq;
using CoreCodedChatbot.Api.Extensions;
using CoreCodedChatbot.Api.Interfaces.Commands;
using CoreCodedChatbot.ApiContract.ResponseModels.DevOps.ChildModels;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;

namespace CoreCodedChatbot.Api.Commands
{
    public class MapWorkItemsAndChildTasksToApiResponseModelsCommand : IMapWorkItemsAndChildTasksToApiResponseModelsCommand
    {
        private readonly IMapWorkItemToTaskCommand _mapWorkItemToTaskCommand;

        public MapWorkItemsAndChildTasksToApiResponseModelsCommand(
            IMapWorkItemToTaskCommand mapWorkItemToTaskCommand
            )
        {
            _mapWorkItemToTaskCommand = mapWorkItemToTaskCommand;
        }

        public DevOpsWorkItem Map(WorkItem parentWorkItem, List<WorkItem> childWorkItems)
        {
            switch (parentWorkItem.WorkItemType())
            {
                case "Product Backlog Item":
                    return new DevOpsProductBacklogItem
                    {
                        Id = parentWorkItem.Id ?? 0,
                        Title = parentWorkItem.Title(),
                        State = parentWorkItem.State(),
                        AssignedTo = parentWorkItem.AssignedTo(),
                        AcceptanceCriteria = parentWorkItem.AcceptanceCriteria(),
                        Description = parentWorkItem.Description(),
                        Tasks = childWorkItems.Select(_mapWorkItemToTaskCommand.Map).Where(t => t != null).ToList()
                    };
                case "Bug":
                    return new DevOpsBug
                    {
                        Id = parentWorkItem.Id ?? 0,
                        Title = parentWorkItem.Title(),
                        State = parentWorkItem.State(),
                        AssignedTo = parentWorkItem.AssignedTo(),
                        AcceptanceCriteria = parentWorkItem.AcceptanceCriteria(),
                        ReproSteps = parentWorkItem.ReproSteps(),
                        SystemInfo = parentWorkItem.SystemInfo(),
                        Tasks = childWorkItems.Select(_mapWorkItemToTaskCommand.Map).Where(t => t != null).ToList()
                    };
                default:
                    return null;
            }
        }
    }
}