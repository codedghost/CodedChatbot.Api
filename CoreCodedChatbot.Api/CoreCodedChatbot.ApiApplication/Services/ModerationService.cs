using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Interfaces.Services;
using CoreCodedChatbot.ApiApplication.Repositories.Users;
using CoreCodedChatbot.Config;
using CoreCodedChatbot.Database.Context.Interfaces;

namespace CoreCodedChatbot.ApiApplication.Services;

public class ModerationService : IBaseService, IModerationService
{
    private readonly IChatbotContextFactory _chatbotContextFactory;
    private readonly IConfigService _configService;

    public ModerationService(IChatbotContextFactory chatbotContextFactory, IConfigService configService)
    {
        _chatbotContextFactory = chatbotContextFactory;
        _configService = configService;
    }

    public async Task TransferUserAccount(string moderationUsername, string oldUsername, string newUsername)
    {
        using (var repo = new UsersRepository(_chatbotContextFactory, _configService))
        {
            await repo.TransferUser(
                moderationUsername.Trim('@'),
                oldUsername.Trim('@'),
                newUsername.Trim('@'));
        }
    }
}