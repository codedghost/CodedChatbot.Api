namespace CoreCodedChatbot.Api.Interfaces.Repositories.Vip
{
    public interface IGetUsersVipCountRepository
    {
        int GetVips(string username);
    }
}