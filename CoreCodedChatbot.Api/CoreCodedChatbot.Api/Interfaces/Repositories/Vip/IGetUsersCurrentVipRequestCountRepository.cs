namespace CoreCodedChatbot.Api.Interfaces.Repositories.Vip
{
    public interface IGetUsersCurrentVipRequestCountRepository
    {
        int GetUsersCurrentVipRequestCount(string username);
    }
}