using System.Linq;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.AzureDevOps;
using CoreCodedChatbot.ApiApplication.Interfaces.Queries.AzureDevOps;
using CoreCodedChatbot.ApiApplication.Interfaces.Services;
using CoreCodedChatbot.ApiContract.ResponseModels.DevOps;

namespace CoreCodedChatbot.ApiApplication.Queries.AzureDevOps;

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