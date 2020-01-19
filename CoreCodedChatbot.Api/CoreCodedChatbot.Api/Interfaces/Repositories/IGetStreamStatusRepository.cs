namespace CoreCodedChatbot.Api.Interfaces.Repositories
{
    public interface IGetStreamStatusRepository
    {
        bool GetStreamStatus(string broadcasterUsername);
    }
}