namespace CoreCodedChatbot.ApiApplication.Interfaces.Commands.ClientId;

public interface IStoreClientIdCommand
{
    void Store(string hubType, string clientId, string username);
}