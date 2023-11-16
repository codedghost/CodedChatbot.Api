namespace CoreCodedChatbot.ApiApplication.Interfaces.Repositories.ClientId;

public interface IStoreClientIdRepository
{
    void Store(string hubType, string username, string clientId);
}