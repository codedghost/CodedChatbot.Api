namespace CoreCodedChatbot.ApiApplication.Interfaces.Repositories.StreamStatus;

public interface IGetStreamStatusRepository
{
    bool GetStreamStatus(string broadcasterUsername);
}