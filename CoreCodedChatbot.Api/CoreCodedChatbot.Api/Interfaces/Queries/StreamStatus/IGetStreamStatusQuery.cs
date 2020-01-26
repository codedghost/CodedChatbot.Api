namespace CoreCodedChatbot.Api.Interfaces.Queries.StreamStatus
{
    public interface IGetStreamStatusQuery
    {
        bool Get(string broadcasterUsername);
    }
}