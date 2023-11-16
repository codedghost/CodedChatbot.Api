using System.Linq;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.AzureDevOps;
using CoreCodedChatbot.ApiApplication.Interfaces.Queries.AzureDevOps;
using CoreCodedChatbot.ApiApplication.Interfaces.Services;
using CoreCodedChatbot.ApiContract.ResponseModels.DevOps;

namespace CoreCodedChatbot.ApiApplication.Queries.AzureDevOps;

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