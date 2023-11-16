using CoreCodedChatbot.ApiContract.Enums.Playlist;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Commands.Playlist;

public interface IUpdatePlaylistStateCommand
{
    bool UpdatePlaylistState(PlaylistState state);
}