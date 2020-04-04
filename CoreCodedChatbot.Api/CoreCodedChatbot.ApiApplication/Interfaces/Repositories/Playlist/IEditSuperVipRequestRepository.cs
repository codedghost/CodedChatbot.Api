namespace CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Playlist
{
    public interface IEditSuperVipRequestRepository
    {
        int Edit(string username, string newText);
    }
}