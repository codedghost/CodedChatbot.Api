using System.Threading.Tasks;
using CoreCodedChatbot.ApiContract.ResponseModels.DevOps.ChildModels;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Queries.AzureDevOps
{
    public interface IGetWorkItemByIdQuery
    {
        Task<DevOpsWorkItem> Get(int id);
    }
}