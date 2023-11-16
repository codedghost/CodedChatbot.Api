namespace CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Vip;

public interface IUseSuperVipRepository
{
    void UseSuperVip(string username, int vipsToUse, int superVipsToRegister);
}