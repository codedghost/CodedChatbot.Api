using System;
using System.Linq;
using System.Threading;
using CodedChatbot.TwitchFactories.Interfaces;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Bytes;
using CoreCodedChatbot.ApiApplication.Interfaces.Queries.StreamStatus;
using CoreCodedChatbot.Config;
using Microsoft.Extensions.Logging;
using TwitchLib.Api.Interfaces;

namespace CoreCodedChatbot.ApiApplication.Services
{
    public interface IChatService
    {
        void Initialise();
    }

    public class ChatService : IChatService
    {
        private readonly IGiveViewershipBytesCommand _giveViewershipBytesCommand;
        private readonly IGetStreamStatusQuery _getStreamStatusQuery;
        private readonly IConfigService _configService;
        private readonly ITwitchAPI _twitchApi;
        private readonly ILogger<IChatService> _logger;
        private Timer _checkChatTimer;

        public ChatService(
            IGiveViewershipBytesCommand giveViewershipBytesCommand,
            IGetStreamStatusQuery getStreamStatusQuery,
            IConfigService configService,
            ITwitchApiFactory twitchApiFactory,
            ILogger<IChatService> logger)
        {
            _giveViewershipBytesCommand = giveViewershipBytesCommand;
            _getStreamStatusQuery = getStreamStatusQuery;
            _configService = configService;
            _twitchApi = twitchApiFactory.Get();
            _logger = logger;
        }

        public void Initialise()
        {
            _checkChatTimer = new Timer(e => { CheckChat(); },
                null,
                TimeSpan.Zero,
                TimeSpan.FromMinutes(1));
        }

        private async void CheckChat()
        {
            try
            {
                var streamerChannel = _configService.Get<string>("StreamerChannel");

                var streamStatus = _getStreamStatusQuery.Get(streamerChannel);

                if (streamStatus)
                {
                    var authToken = await _twitchApi.Helix.Users.GetUsersAsync();
                    var chattersResponse = await _twitchApi.Helix.Chat.GetChattersAsync(authToken.Users.FirstOrDefault()?.Id, authToken.Users.FirstOrDefault()?.Id, 1000);

                    if (!chattersResponse.Data.Any()) return;

                    var chatters = chattersResponse.Data.Select(d => d.UserLogin).ToList();
                    
                    _giveViewershipBytesCommand.Give(chatters);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Couldn't access the TMI Service");
            }
        }
    }
}