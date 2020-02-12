namespace CoreCodedChatbot.Api.Interfaces.Commands.Playlist
{
    public interface IAddSongToDriveCommand
    {
        bool AddSongToDrive(int songRequestId);
    }
}