using CoreCodedChatbot.ApiContract.Enums.Playlist;

namespace CoreCodedChatbot.Api.Interfaces.Repositories.Playlist
{
    public interface IGetPlaylistStateRepository
    {
        PlaylistState GetPlaylistState();
    }
}