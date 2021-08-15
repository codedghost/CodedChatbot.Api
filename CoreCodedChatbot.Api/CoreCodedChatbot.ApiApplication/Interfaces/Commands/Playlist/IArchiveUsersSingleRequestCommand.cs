using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Models.Enums;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Commands.Playlist
{
    public interface IArchiveUsersSingleRequestCommand
    {
        Task<bool> ArchiveAndRefundVips(string username, SongRequestType songRequestType, int currentSongRequestId);
    }
}