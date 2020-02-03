namespace CoreCodedChatbot.Api.Interfaces.Commands.Vip
{
    public interface IUseVipCommand
    {
        void UseVip(string username, int vips);
    }
}