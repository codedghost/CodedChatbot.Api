using CoreCodedChatbot.Api.Models.Enums;
using CoreCodedChatbot.Api.Models.Intermediates;

namespace CoreCodedChatbot.Api.Interfaces.Commands.Playlist
{
    public interface IAddSongRequestCommand
    {
        AddSongResult AddSongRequest(string username, string requestText, SongRequestType songRequestType);
    }
}