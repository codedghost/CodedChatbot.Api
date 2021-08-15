namespace CoreCodedChatbot.ApiApplication.Interfaces.Repositories.ClientId
{
    public interface IRemoveClientIdRepository
    {
        void Remove(string hubType, string clientId);
    }
}