using CoreCodedChatbot.Api.Models.Enums;
using CoreCodedChatbot.Api.Models.Intermediates;
using CoreCodedChatbot.ApiContract.Enums.Playlist;

namespace CoreCodedChatbot.Api.Interfaces.Commands.Playlist
{
    public interface IAddSongRequestCommand
    {
        AddSongResult AddSongRequest(string username, string requestText, SongRequestType songRequestType);
    }
}