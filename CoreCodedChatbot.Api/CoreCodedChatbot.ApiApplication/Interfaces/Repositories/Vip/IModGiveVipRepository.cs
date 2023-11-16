namespace CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Vip;

public interface IModGiveVipRepository
{
    void ModGiveVip(string username, int vipsToGive);
}