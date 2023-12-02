using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CodedChatbot.ServiceBusContract;
using CodedChatbot.TwitchFactories.Interfaces;
using CodedGhost.AzureServiceBus.Abstractions;
using CoreCodedChatbot.ApiApplication.Interfaces.Queries.StreamStatus;
using CoreCodedChatbot.ApiApplication.Interfaces.Services;
using CoreCodedChatbot.ApiApplication.Repositories.Users;
using CoreCodedChatbot.Config;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Secrets;
using Microsoft.Extensions.Logging;
using TwitchLib.Api.Interfaces;

namespace CoreCodedChatbot.ApiApplication.Services;

public class ChatService : IBaseService, IChatService
{
    private readonly IChatbotContextFactory _chatbotContextFactory;
    private readonly IGetStreamStatusQuery _getStreamStatusQuery;
    private readonly IConfigService _configService;
    private readonly ISecretService _secretService;
    private readonly ITwitchAPI _twitchApi;
    private readonly ILogger<IChatService> _logger;
    private Timer _checkChatTimer;

    public ChatService(
        IChatbotContextFactory chatbotContextFactory,
        IGetStreamStatusQuery getStreamStatusQuery,
        IConfigService configService,
        ISecretService secretService,
        ITwitchApiFactory twitchApiFactory,
        ILogger<IChatService> logger)
    {
        _chatbotContextFactory = chatbotContextFactory;
        _getStreamStatusQuery = getStreamStatusQuery;
        _configService = configService;
        _secretService = secretService;
        _twitchApi = twitchApiFactory.Get();
        _logger = logger;
    }

    public void Initialise()
    {
        _checkChatTimer = new Timer(e => { CheckChat().Wait(); },
            null,
            TimeSpan.Zero,
            TimeSpan.FromMinutes(1));
    }

    public async Task<bool> SendMessage(string message)
    {
        var sender = new CodedServiceBusSender<TwitchChat>(_secretService);

        await sender.SendMessageAsync(new TwitchChat
        {
            SendingApplication = "CodedChatbot.Api",
            Message = message
        });

        return true;
    }

    private async Task CheckChat()
    {
        try
        {
            var streamerChannel = _configService.Get<string>("StreamerChannel");

            var streamStatus = _getStreamStatusQuery.Get(streamerChannel);

            if (streamStatus)
            {
                var loggedInUser = await _twitchApi.Helix.Users.GetUsersAsync();
                var chattersResponse = await _twitchApi.Helix.Chat.GetChattersAsync(loggedInUser.Users.FirstOrDefault()?.Id, loggedInUser.Users.FirstOrDefault()?.Id, 1000);

                if (!chattersResponse.Data.Any()) return;

                var chatters = chattersResponse.Data.Select(d => d.UserLogin).ToList();

                using (var repo = new UsersRepository(_chatbotContextFactory))
                {
                    await repo.UpdateWatchTime(chatters);
                }
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Couldn't access the TMI Service");
        }
    }
}