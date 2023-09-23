using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CodedChatbot.TwitchFactories.Interfaces;
using CoreCodedChatbot.ApiApplication.Interfaces.Queries.StreamStatus;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Bytes;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.WatchTime;
using CoreCodedChatbot.ApiApplication.Interfaces.Services;
using CoreCodedChatbot.Config;
using Microsoft.Extensions.Logging;
using TwitchLib.Api.Interfaces;

namespace CoreCodedChatbot.ApiApplication.Services
{
    public class ChatService : IChatService
    {
        private readonly IGiveViewershipBytesRepository _giveViewershipBytesRepository;
        private readonly IUpdateWatchTimeRepository _updateWatchTimeRepository;
        private readonly IGetStreamStatusQuery _getStreamStatusQuery;
        private readonly IConfigService _configService;
        private readonly ITwitchAPI _twitchApi;
        private readonly ILogger<IChatService> _logger;
        private Timer _checkChatTimer;

        public ChatService(
            IGiveViewershipBytesRepository giveViewershipBytesRepository,
            IUpdateWatchTimeRepository updateWatchTimeRepository,
            IGetStreamStatusQuery getStreamStatusQuery,
            IConfigService configService,
            ITwitchApiFactory twitchApiFactory,
            ILogger<IChatService> logger)
        {
            _giveViewershipBytesRepository = giveViewershipBytesRepository;
            _updateWatchTimeRepository = updateWatchTimeRepository;
            _getStreamStatusQuery = getStreamStatusQuery;
            _configService = configService;
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
                    
                    await _giveViewershipBytesRepository.Give(chatters);
                    await _updateWatchTimeRepository.Update(chatters);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Couldn't access the TMI Service");
            }
        }
    }
}