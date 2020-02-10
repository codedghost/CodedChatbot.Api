namespace CoreCodedChatbot.Api.Interfaces.Repositories.Playlist
{
    public interface IGetUsersCurrentRequestCountRepository
    {
        int GetUsersCurrentRequestCount(string username);
    }
}