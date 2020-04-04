using CoreCodedChatbot.ApiApplication.Models.Enums;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Queries.Playlist
{
    public interface IGetSingleSongRequestIdQuery
    {
        int Get(string username, SongRequestType songRequestType);
    }
}