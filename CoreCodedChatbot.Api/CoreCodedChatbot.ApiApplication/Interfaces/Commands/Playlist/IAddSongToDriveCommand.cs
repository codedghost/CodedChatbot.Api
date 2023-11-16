namespace CoreCodedChatbot.ApiApplication.Interfaces.Commands.Playlist;

public interface IAddSongToDriveCommand
{
    bool AddSongToDrive(int songRequestId);
}