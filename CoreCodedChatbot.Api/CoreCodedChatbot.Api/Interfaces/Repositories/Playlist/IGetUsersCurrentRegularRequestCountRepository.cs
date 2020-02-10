namespace CoreCodedChatbot.Api.Interfaces.Repositories.Playlist
{
    public interface IGetUsersCurrentRegularRequestCountRepository
    {
        int GetUsersCurrentRegularRequestCount(string username);
    }
}