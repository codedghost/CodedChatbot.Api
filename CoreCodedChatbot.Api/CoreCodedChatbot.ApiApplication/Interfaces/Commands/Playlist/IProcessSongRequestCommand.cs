using CoreCodedChatbot.ApiApplication.Models.Enums;
using CoreCodedChatbot.ApiApplication.Models.Intermediates;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Commands.Playlist
{
    public interface IProcessSongRequestCommand
    {
        AddSongResult ProcessAddingSongRequest(string username, string requestText,
            SongRequestType songRequestType);
    }
}