namespace CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Vip;

public interface IGetUsersVipCountRepository
{
    int GetVips(string username);
}