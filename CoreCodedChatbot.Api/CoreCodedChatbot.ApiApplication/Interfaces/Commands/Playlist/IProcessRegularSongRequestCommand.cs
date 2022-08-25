using CoreCodedChatbot.ApiApplication.Models.Intermediates;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Commands.Playlist
{
    public interface IProcessRegularSongRequestCommand
    {
        AddSongResult Process(string username, string requestText, int searchSongId);
    }
}