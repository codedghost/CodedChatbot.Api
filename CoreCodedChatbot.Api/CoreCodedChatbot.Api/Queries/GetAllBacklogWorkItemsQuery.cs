using System.Linq;
using System.Threading.Tasks;
using CoreCodedChatbot.Api.Interfaces.Commands;
using CoreCodedChatbot.Api.Interfaces.Services;
using CoreCodedChatbot.ApiContract.ResponseModels.DevOps;

namespace CoreCodedChatbot.Api.Queries
{
    public class GetAllBacklogWorkItemsQuery : IGetAllBacklogWorkItemsQuery
    {
        private readonly IAzureDevOpsService _azureDevOpsService;
        private readonly IMapWorkItemToParentWorkItemCommand _mapWorkItemToParentWorkItemCommand;

        public GetAllBacklogWorkItemsQuery(
            IAzureDevOpsService azureDevOpsService,
            IMapWorkItemToParentWorkItemCommand mapWorkItemToParentWorkItemCommand
            )
        {
            _azureDevOpsService = azureDevOpsService;
            _mapWorkItemToParentWorkItemCommand = mapWorkItemToParentWorkItemCommand;
        }

        public async Task<GetAllBacklogWorkItemsResponse> Get()
        {
            var backlogItems = await _azureDevOpsService.GetBacklogWorkItems();

            var mappedItems = backlogItems.Select(_mapWorkItemToParentWorkItemCommand.Map);

            return new GetAllBacklogWorkItemsResponse
            {
                WorkItems = mappedItems.ToList()
            };
        }
    }
}