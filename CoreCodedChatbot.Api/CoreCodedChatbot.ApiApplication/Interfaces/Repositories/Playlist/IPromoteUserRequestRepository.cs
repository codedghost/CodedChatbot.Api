namespace CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Playlist
{
    public interface IPromoteUserRequestRepository
    {
        int PromoteUserRequest(string username, int songRequestId);
    }
}