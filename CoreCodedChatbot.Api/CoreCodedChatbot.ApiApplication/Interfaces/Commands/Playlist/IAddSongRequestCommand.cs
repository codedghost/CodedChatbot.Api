using CoreCodedChatbot.ApiApplication.Models.Enums;
using CoreCodedChatbot.ApiApplication.Models.Intermediates;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Commands.Playlist
{
    public interface IAddSongRequestCommand
    {
        AddSongResult AddSongRequest(string username, string requestText, SongRequestType songRequestType);
    }
}