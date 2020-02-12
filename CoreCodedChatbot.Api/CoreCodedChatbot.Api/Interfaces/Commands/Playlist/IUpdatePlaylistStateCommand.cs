using CoreCodedChatbot.ApiContract.Enums.Playlist;

namespace CoreCodedChatbot.Api.Interfaces.Commands.Playlist
{
    public interface IUpdatePlaylistStateCommand
    {
        bool UpdatePlaylistState(PlaylistState state);
    }
}