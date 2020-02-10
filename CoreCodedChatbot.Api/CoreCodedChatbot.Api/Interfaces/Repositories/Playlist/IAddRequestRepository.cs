using CoreCodedChatbot.Api.Models.Intermediates;

namespace CoreCodedChatbot.Api.Interfaces.Repositories.Playlist
{
    public interface IAddRequestRepository
    {
        AddSongResult AddRequest(string requestText, string username, bool isVip, bool isSuperVip);
    }
}