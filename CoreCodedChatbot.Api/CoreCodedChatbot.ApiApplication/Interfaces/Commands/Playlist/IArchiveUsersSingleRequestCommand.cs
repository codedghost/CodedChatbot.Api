using CoreCodedChatbot.ApiApplication.Models.Enums;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Commands.Playlist
{
    public interface IArchiveUsersSingleRequestCommand
    {
        bool ArchiveAndRefundVips(string username, SongRequestType songRequestType, int currentSongRequestId);
    }
}