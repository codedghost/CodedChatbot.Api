namespace CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Playlist;

public interface IGetUsersCurrentRequestCountRepository
{
    int GetUsersCurrentRequestCount(string username);
}