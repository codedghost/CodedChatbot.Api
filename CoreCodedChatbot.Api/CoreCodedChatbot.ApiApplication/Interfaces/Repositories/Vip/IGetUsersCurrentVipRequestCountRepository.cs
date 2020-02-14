namespace CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Vip
{
    public interface IGetUsersCurrentVipRequestCountRepository
    {
        int GetUsersCurrentVipRequestCount(string username);
    }
}