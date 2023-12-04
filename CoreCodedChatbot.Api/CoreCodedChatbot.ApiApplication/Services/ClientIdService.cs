using System.Collections.Generic;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Extensions;
using CoreCodedChatbot.ApiApplication.Interfaces.Services;
using CoreCodedChatbot.ApiApplication.Repositories.Users;
using CoreCodedChatbot.Config;
using CoreCodedChatbot.Database.Context.Interfaces;
using Microsoft.Extensions.Logging;

namespace CoreCodedChatbot.ApiApplication.Services;

public class ClientIdService : IBaseService, IClientIdService
{
    private readonly IChatbotContextFactory _chatbotContextFactory;
    private readonly IConfigService _configService;
    private readonly ILogger<ClientIdService> _logger;

    public ClientIdService(
        IChatbotContextFactory chatbotContextFactory,
        IConfigService configService,
        ILogger<ClientIdService> logger
    )
    {
        _chatbotContextFactory = chatbotContextFactory;
        _configService = configService;
        _logger = logger;
    }

    public async Task SaveClientId(string hubType, string clientId, string username)
    {
        hubType.AssertNonNull();
        clientId.AssertNonNull();
        username.AssertNonNull();

        using (var repo = new UsersRepository(_chatbotContextFactory, _configService, _logger))
        {
            await repo.StoreClientId(hubType, clientId, username);
        }
    }

    public async Task RemoveClientId(string hubType, string clientId)
    {
        hubType.AssertNonNull();
        clientId.AssertNonNull();

        using (var repo = new UsersRepository(_chatbotContextFactory, _configService, _logger))
        {
            await repo.RemoveClientId(hubType, clientId);
        }
    }

    public async Task<List<string>> GetClientIds(string username, string hubType)
    {
        using (var repo = new UsersRepository(_chatbotContextFactory, _configService, _logger))
        {
            return await repo.GetClientIds(username, hubType);
        }
    }
}