namespace CoreCodedChatbot.ApiApplication.Interfaces.Commands.Playlist;

public interface IEditSuperVipCommand
{
    int Edit(string username, string newText, int songId);
}