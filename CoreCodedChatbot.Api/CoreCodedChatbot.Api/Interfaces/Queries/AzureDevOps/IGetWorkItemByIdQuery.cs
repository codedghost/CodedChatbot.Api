using System.Threading.Tasks;
using CoreCodedChatbot.ApiContract.ResponseModels.DevOps.ChildModels;

namespace CoreCodedChatbot.Api.Interfaces.Queries.AzureDevOps
{
    public interface IGetWorkItemByIdQuery
    {
        Task<DevOpsWorkItem> Get(int id);
    }
}