namespace CoreCodedChatbot.Api.Interfaces.Repositories.StreamStatus
{
    public interface IGetStreamStatusRepository
    {
        bool GetStreamStatus(string broadcasterUsername);
    }
}