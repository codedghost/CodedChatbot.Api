using System.Threading.Tasks;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Commands.Playlist
{
    public interface IRemoveUsersRequestByPlaylistIndexCommand
    {
        Task<bool> Remove(string username, int playlistPosition);
    }
}