using CoreCodedChatbot.ApiContract.Enums.Playlist;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Queries.Playlist
{
    public interface IGetPlaylistStateQuery
    {
        PlaylistState GetPlaylistState();
    }
}