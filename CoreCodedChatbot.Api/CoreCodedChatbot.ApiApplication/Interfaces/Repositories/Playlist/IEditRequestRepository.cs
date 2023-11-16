namespace CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Playlist;

public interface IEditRequestRepository
{
    void Edit(int songRequestId, string requestText, string username, bool isMod, int songId);
}