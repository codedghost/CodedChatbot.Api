using CoreCodedChatbot.ApiApplication.Models.Intermediates;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Queries.Playlist
{
    public interface IGetUsersRequestAtPlaylistIndexQuery
    {
        BasicSongRequest Get(string username, int index, bool isCurrentRequestVip);
    }
}