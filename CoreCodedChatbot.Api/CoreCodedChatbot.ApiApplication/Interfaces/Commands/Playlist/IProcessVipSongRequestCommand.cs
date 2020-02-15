using CoreCodedChatbot.ApiApplication.Models.Intermediates;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Commands.Playlist
{
    public interface IProcessVipSongRequestCommand
    {
        AddSongResult Process(string username, string requestText);
    }
}