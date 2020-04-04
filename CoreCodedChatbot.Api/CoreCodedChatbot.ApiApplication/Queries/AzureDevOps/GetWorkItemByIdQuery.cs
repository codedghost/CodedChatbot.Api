using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Extensions;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.AzureDevOps;
using CoreCodedChatbot.ApiApplication.Interfaces.Queries.AzureDevOps;
using CoreCodedChatbot.ApiApplication.Interfaces.Services;
using CoreCodedChatbot.ApiContract.ResponseModels.DevOps.ChildModels;

namespace CoreCodedChatbot.ApiApplication.Queries.AzureDevOps
{
    public class GetWorkItemByIdQuery : IGetWorkItemByIdQuery
    {
        private readonly IAzureDevOpsService _azureDevOpsService;
        private readonly IMapWorkItemToParentWorkItemCommand _mapWorkItemToParentWorkItemCommand;
        private readonly IMapWorkItemToTaskCommand _mapWorkItemToTaskCommand;

        public GetWorkItemByIdQuery(
            IAzureDevOpsService azureDevOpsService,
            IMapWorkItemToParentWorkItemCommand mapWorkItemToParentWorkItemCommand,
            IMapWorkItemToTaskCommand mapWorkItemToTaskCommand
            )
        {
            _azureDevOpsService = azureDevOpsService;
            _mapWorkItemToParentWorkItemCommand = mapWorkItemToParentWorkItemCommand;
            _mapWorkItemToTaskCommand = mapWorkItemToTaskCommand;
        }
        
        public async Task<DevOpsWorkItem> Get(int id)
        {
            var workItem = await _azureDevOpsService.GetWorkItemById(id);

            var mappedWorkItem = workItem.WorkItemType() != "Task"
                ? _mapWorkItemToParentWorkItemCommand.Map(workItem)
                : _mapWorkItemToTaskCommand.Map(workItem);

            return mappedWorkItem;
        }
    }
}