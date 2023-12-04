using System;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Interfaces.Services;
using CoreCodedChatbot.ApiApplication.Repositories.Users;
using CoreCodedChatbot.Config;
using CoreCodedChatbot.Database.Context.Interfaces;
using Microsoft.Extensions.Logging;

namespace CoreCodedChatbot.ApiApplication.Services;

public class WatchTimeService : IBaseService, IWatchTimeService
{
    private readonly IChatbotContextFactory _chatbotContextFactory;
    private readonly IConfigService _configService;
    private readonly ILogger<IWatchTimeService> _logger;

    public WatchTimeService(
        IChatbotContextFactory chatbotContextFactory,
        IConfigService configService,
        ILogger<IWatchTimeService> logger
    )
    {
        _chatbotContextFactory = chatbotContextFactory;
        _configService = configService;
        _logger = logger;
    }

    public async Task<TimeSpan> GetWatchTime(string username)
    {
        using (var repo = new UsersRepository(_chatbotContextFactory, _configService, _logger))
        {
            return await repo.GetWatchTime(username);
        }
    }
}