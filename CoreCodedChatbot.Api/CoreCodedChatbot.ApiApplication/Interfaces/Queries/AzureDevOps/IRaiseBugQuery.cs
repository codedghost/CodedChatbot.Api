using System.Threading.Tasks;
using CoreCodedChatbot.ApiContract.ResponseModels.DevOps.ChildModels;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Queries.AzureDevOps
{
    public interface IRaiseBugQuery
    {
        Task<bool> Raise(string twitchUsername, DevOpsBug bugInfo);
    }
}