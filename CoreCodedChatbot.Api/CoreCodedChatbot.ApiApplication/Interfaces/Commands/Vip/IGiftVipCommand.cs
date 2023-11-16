namespace CoreCodedChatbot.ApiApplication.Interfaces.Commands.Vip;

public interface IGiftVipCommand
{
    bool GiftVip(string donorUsername, string receivingUsername, int vipsToGift);
}