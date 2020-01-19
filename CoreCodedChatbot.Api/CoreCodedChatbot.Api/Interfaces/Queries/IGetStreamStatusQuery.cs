namespace CoreCodedChatbot.Api.Interfaces.Queries
{
    public interface IGetStreamStatusQuery
    {
        bool Get(string broadcasterUsername);
    }
}