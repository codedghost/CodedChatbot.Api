using System.Threading.Tasks;
using CoreCodedChatbot.ApiContract.ResponseModels.DevOps;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Queries.AzureDevOps
{
    public interface IGetAllBacklogWorkItemsQuery
    {
        Task<GetAllBacklogWorkItemsResponse> Get();
    }
}