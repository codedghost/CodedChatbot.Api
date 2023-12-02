using System.Collections.Generic;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Extensions;
using CoreCodedChatbot.ApiApplication.Interfaces.Services;
using CoreCodedChatbot.ApiApplication.Repositories.Users;
using CoreCodedChatbot.Database.Context.Interfaces;

namespace CoreCodedChatbot.ApiApplication.Services;

public class ClientIdService : IBaseService, IClientIdService
{
    private readonly IChatbotContextFactory _chatbotContextFactory;

    public ClientIdService(
        IChatbotContextFactory chatbotContextFactory
        )
    {
        _chatbotContextFactory = chatbotContextFactory;
    }

    public async Task SaveClientId(string hubType, string clientId, string username)
    {
        hubType.AssertNonNull();
        clientId.AssertNonNull();
        username.AssertNonNull();

        using (var repo = new UsersRepository(_chatbotContextFactory))
        {
            await repo.StoreClientId(hubType, clientId, username);
        }
    }

    public async Task RemoveClientId(string hubType, string clientId)
    {
        hubType.AssertNonNull();
        clientId.AssertNonNull();

        using (var repo = new UsersRepository(_chatbotContextFactory))
        {
            await repo.RemoveClientId(hubType, clientId);
        }
    }

    public async Task<List<string>> GetClientIds(string username, string hubType)
    {
        using (var repo = new UsersRepository(_chatbotContextFactory))
        {
            return await repo.GetClientIds(username, hubType);
        }
    }
}