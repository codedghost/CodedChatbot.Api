namespace CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Vip
{
    public interface IGiftVipRepository
    {
        void GiftVip(string donorUsername, string receivingUsername, int vipsToGift);
    }
}