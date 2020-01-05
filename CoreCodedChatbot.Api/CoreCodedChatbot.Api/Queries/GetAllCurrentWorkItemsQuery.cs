using System.Linq;
using System.Threading.Tasks;
using CoreCodedChatbot.Api.Interfaces.Commands;
using CoreCodedChatbot.Api.Interfaces.Queries;
using CoreCodedChatbot.Api.Interfaces.Services;
using CoreCodedChatbot.ApiContract.ResponseModels.DevOps;

namespace CoreCodedChatbot.Api.Queries
{
    public class GetAllCurrentWorkItemsQuery : IGetAllCurrentWorkItemsQuery
    {
        private readonly IAzureDevOpsService _azureDevOpsService;
        private readonly IMapWorkItemToParentWorkItemCommand _mapWorkItemToParentWorkItemCommand;

        public GetAllCurrentWorkItemsQuery(
            IAzureDevOpsService azureDevOpsService,
            IMapWorkItemToParentWorkItemCommand mapWorkItemToParentWorkItemCommand 
            )
        {
            _azureDevOpsService = azureDevOpsService;
            _mapWorkItemToParentWorkItemCommand = mapWorkItemToParentWorkItemCommand;
        }

        public async Task<GetAllCurrentWorkItemsResponse> Get()
        {
            var currentPbis = await _azureDevOpsService.GetCommittedPbisForThisIteration();

            var mappedPbis = currentPbis.Select(_mapWorkItemToParentWorkItemCommand.Map);

            return new GetAllCurrentWorkItemsResponse
            {
                WorkItems = mappedPbis.ToList()
            };
        }
    }
}