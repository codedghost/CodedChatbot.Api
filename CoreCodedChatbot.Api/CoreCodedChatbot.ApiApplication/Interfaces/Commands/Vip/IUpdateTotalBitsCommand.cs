namespace CoreCodedChatbot.ApiApplication.Interfaces.Commands.Vip
{
    public interface IUpdateTotalBitsCommand
    {
        void Update(string username, int totalBits);
    }
}