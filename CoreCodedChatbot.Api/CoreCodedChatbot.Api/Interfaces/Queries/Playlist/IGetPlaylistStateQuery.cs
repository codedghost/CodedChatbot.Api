using CoreCodedChatbot.ApiContract.Enums.Playlist;

namespace CoreCodedChatbot.Api.Interfaces.Queries.Playlist
{
    public interface IGetPlaylistStateQuery
    {
        PlaylistState GetPlaylistState();
    }
}