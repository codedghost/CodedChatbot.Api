using CoreCodedChatbot.Api.Models.Intermediates;

namespace CoreCodedChatbot.Api.Interfaces.Repositories.Playlist
{
    public interface IGetCurrentRequestsRepository
    {
        CurrentRequestsIntermediate GetCurrentRequests();
    }
}