using CoreCodedChatbot.ApiApplication.Models.Intermediates;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Playlist
{
    public interface IAddRequestRepository
    {
        AddSongResult AddRequest(string requestText, string username, bool isVip, bool isSuperVip);
    }
}