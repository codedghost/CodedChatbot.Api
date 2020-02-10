namespace CoreCodedChatbot.Api.Interfaces.Repositories.Playlist
{
    public interface IGetIsUserInChatRepository
    {
        bool IsUserInChat(string username);
    }
}