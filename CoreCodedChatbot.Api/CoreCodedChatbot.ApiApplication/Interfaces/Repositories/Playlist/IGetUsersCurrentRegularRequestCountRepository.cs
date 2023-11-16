namespace CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Playlist;

public interface IGetUsersCurrentRegularRequestCountRepository
{
    int GetUsersCurrentRegularRequestCount(string username);
}