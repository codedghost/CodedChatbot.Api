namespace CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Bytes
{
    public interface IGiveGiftSubBytesRepository
    {
        void Give(string username, int conversionAmount);
    }
}