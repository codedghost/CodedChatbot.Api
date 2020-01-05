using System.Threading.Tasks;
using CoreCodedChatbot.ApiContract.ResponseModels.DevOps;

namespace CoreCodedChatbot.Api.Interfaces.Queries
{
    public interface IGetAllCurrentWorkItemsQuery
    {
        Task<GetAllCurrentWorkItemsResponse> Get();
    }
}