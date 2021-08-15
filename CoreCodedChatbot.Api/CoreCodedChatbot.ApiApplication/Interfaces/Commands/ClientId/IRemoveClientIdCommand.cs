namespace CoreCodedChatbot.ApiApplication.Interfaces.Commands.ClientId
{
    public interface IRemoveClientIdCommand
    {
        void Remove(string hubType, string clientId);
    }
}