using CoreCodedChatbot.ApiApplication.Models.Intermediates;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Commands.Playlist
{
    public interface IProcessSuperVipSongRequestCommand
    {
        AddSongResult Process(string username, string requestText);
    }
}