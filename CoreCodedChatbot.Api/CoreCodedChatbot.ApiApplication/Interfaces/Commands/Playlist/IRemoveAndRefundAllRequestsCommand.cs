using System.Threading.Tasks;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Commands.Playlist;

public interface IRemoveAndRefundAllRequestsCommand
{
    Task RemoveAndRefundAllRequests();
}