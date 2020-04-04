using CoreCodedChatbot.ApiApplication.Interfaces.Commands.AzureDevOps;
using CoreCodedChatbot.ApiApplication.Interfaces.Services;
using CoreCodedChatbot.ApiContract.ResponseModels.DevOps.ChildModels;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;

namespace CoreCodedChatbot.ApiApplication.Commands.AzureDevOps
{
    public class MapWorkItemToParentWorkItemCommand : IMapWorkItemToParentWorkItemCommand
    {
        private readonly IAzureDevOpsService _azureDevOpsService;
        private readonly IMapWorkItemsAndChildTasksToApiResponseModelsCommand _mapWorkItemsAndChildTasksToApiResponseModelsCommand;

        public MapWorkItemToParentWorkItemCommand(
            IAzureDevOpsService azureDevOpsService,
            IMapWorkItemsAndChildTasksToApiResponseModelsCommand mapWorkItemsAndChildTasksToApiResponseModelsCommand
            )
        {
            _azureDevOpsService = azureDevOpsService;
            _mapWorkItemsAndChildTasksToApiResponseModelsCommand = mapWorkItemsAndChildTasksToApiResponseModelsCommand;
        }

        public DevOpsWorkItem Map(WorkItem parentWorkItem)
        {
            var workItems = _azureDevOpsService.GetChildWorkItemsByPbi(parentWorkItem);

            var mapParentWorkItem = _mapWorkItemsAndChildTasksToApiResponseModelsCommand.Map(parentWorkItem, workItems.Result);

            return mapParentWorkItem;
        }
    }
}