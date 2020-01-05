using System.Threading.Tasks;
using CoreCodedChatbot.ApiContract.ResponseModels.DevOps.ChildModels;

namespace CoreCodedChatbot.Api.Interfaces.Queries
{
    public interface IRaiseBugQuery
    {
        Task<bool> Raise(string twitchUsername, DevOpsBug bugInfo);
    }
}