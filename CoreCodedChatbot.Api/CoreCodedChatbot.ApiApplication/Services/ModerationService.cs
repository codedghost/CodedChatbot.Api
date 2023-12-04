using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Interfaces.Services;
using CoreCodedChatbot.ApiApplication.Repositories.Users;
using CoreCodedChatbot.Config;
using CoreCodedChatbot.Database.Context.Interfaces;
using Microsoft.Extensions.Logging;

namespace CoreCodedChatbot.ApiApplication.Services;

public class ModerationService : IBaseService, IModerationService
{
    private readonly IChatbotContextFactory _chatbotContextFactory;
    private readonly IConfigService _configService;
    private readonly ILogger<ModerationService> _logger;

    public ModerationService(
        IChatbotContextFactory chatbotContextFactory,
        IConfigService configService,
        ILogger<ModerationService> logger)
    {
        _chatbotContextFactory = chatbotContextFactory;
        _configService = configService;
        _logger = logger;
    }

    public async Task TransferUserAccount(string moderationUsername, string oldUsername, string newUsername)
    {
        using (var repo = new UsersRepository(_chatbotContextFactory, _configService, _logger))
        {
            await repo.TransferUser(
                moderationUsername.Trim('@'),
                oldUsername.Trim('@'),
                newUsername.Trim('@'));
        }
    }
}