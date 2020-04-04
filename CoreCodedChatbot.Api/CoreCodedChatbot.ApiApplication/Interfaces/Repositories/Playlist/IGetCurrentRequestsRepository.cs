using CoreCodedChatbot.ApiApplication.Models.Intermediates;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Playlist
{
    public interface IGetCurrentRequestsRepository
    {
        CurrentRequestsIntermediate GetCurrentRequests();
    }
}