namespace CoreCodedChatbot.ApiApplication.Interfaces.Commands.Playlist
{
    public interface IRemoveUsersRequestByPlaylistIndexCommand
    {
        bool Remove(string username, int playlistPosition);
    }
}