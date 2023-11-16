namespace CoreCodedChatbot.ApiApplication.Interfaces.Queries.StreamStatus;

public interface IGetStreamStatusQuery
{
    bool Get(string broadcasterUsername);
}