namespace CoreCodedChatbot.Api.Interfaces.Repositories.Vip
{
    public interface IGiftVipRepository
    {
        void GiftVip(string donorUsername, string receivingUsername);
    }
}