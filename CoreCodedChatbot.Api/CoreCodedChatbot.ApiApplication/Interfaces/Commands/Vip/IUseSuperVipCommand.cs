namespace CoreCodedChatbot.ApiApplication.Interfaces.Commands.Vip
{
    public interface IUseSuperVipCommand
    {
        void UseSuperVip(string username, int discount);
    }
}