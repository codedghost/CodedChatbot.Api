namespace CoreCodedChatbot.Api.Interfaces.Commands.Quote
{
    public interface IGiftVipCommand
    {
        bool GiftVip(string donorUsername, string receivingUsername, int vipsToGift);
    }
}