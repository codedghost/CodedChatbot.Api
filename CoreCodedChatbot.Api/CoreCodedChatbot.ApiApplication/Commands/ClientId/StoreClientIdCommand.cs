using CoreCodedChatbot.ApiApplication.Extensions;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.ClientId;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.ClientId;

namespace CoreCodedChatbot.ApiApplication.Commands.ClientId;

public class StoreClientIdCommand : IStoreClientIdCommand
{
    private readonly IStoreClientIdRepository _storeClientIdRepository;

    public StoreClientIdCommand(IStoreClientIdRepository storeClientIdRepository)
    {
        _storeClientIdRepository = storeClientIdRepository;
    }

    public void Store(string hubType, string clientId, string username)
    {
        hubType.AssertNonNull();
        clientId.AssertNonNull();
        username.AssertNonNull();

        _storeClientIdRepository.Store(hubType, username, clientId);
    }
}