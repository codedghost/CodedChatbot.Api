namespace CoreCodedChatbot.Api.Interfaces.Commands.Vip
{
    public interface IModGiveVipCommand
    {
        void ModGiveVip(string username, int vipsToGive);
    }
}