using System.Threading.Tasks;
using CoreCodedChatbot.ApiContract.ResponseModels.DevOps;

namespace CoreCodedChatbot.Api.Queries
{
    public interface IGetAllBacklogWorkItemsQuery
    {
        Task<GetAllBacklogWorkItemsResponse> Get();
    }
}