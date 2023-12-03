using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Settings;
using CoreCodedChatbot.ApiApplication.Interfaces.Services;
using CoreCodedChatbot.ApiApplication.Repositories.Settings;
using CoreCodedChatbot.Database.Context.Interfaces;

namespace CoreCodedChatbot.ApiApplication.Services;

public class SettingsService : IBaseService, ISettingsService
{
    private readonly IChatbotContextFactory _chatbotContextFactory;

    public SettingsService(IChatbotContextFactory chatbotContextFactory)
    {
        _chatbotContextFactory = chatbotContextFactory;
    }

    public async Task Update(string key, string value)
    {
        using (var repo = new SettingsRepository(_chatbotContextFactory))
        {
            await repo.Set(key, value);
        }
    }
}