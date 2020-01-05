using System.Threading.Tasks;
using CoreCodedChatbot.ApiContract.ResponseModels.DevOps.ChildModels;

namespace CoreCodedChatbot.Api.Interfaces.Queries
{
    public interface IGetWorkItemByIdQuery
    {
        Task<DevOpsWorkItem> Get(int id);
    }
}