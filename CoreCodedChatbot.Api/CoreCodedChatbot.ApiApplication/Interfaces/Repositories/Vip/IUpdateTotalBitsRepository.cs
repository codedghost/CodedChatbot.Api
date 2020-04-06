namespace CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Vip
{
    public interface IUpdateTotalBitsRepository
    {
        void Update(string username, int totalBits);
    }
}