using System;
using System.Net.Http;
using System.Threading;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Bytes;
using CoreCodedChatbot.ApiApplication.Interfaces.Queries.StreamStatus;
using CoreCodedChatbot.ApiApplication.Models.Intermediates;
using CoreCodedChatbot.Config;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

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
        private readonly ILogger<IChatService> _logger;
        private Timer _checkChatTimer;

        public ChatService(
            IGiveViewershipBytesCommand giveViewershipBytesCommand,
            IGetStreamStatusQuery getStreamStatusQuery,
            IConfigService configService,
            ILogger<IChatService> logger)
        {
            _giveViewershipBytesCommand = giveViewershipBytesCommand;
            _getStreamStatusQuery = getStreamStatusQuery;
            _configService = configService;
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

                    var client = new HttpClient();

                    var currentChatters =
                        await client.GetAsync($"https://tmi.twitch.tv/group/user/{streamerChannel}/chatters");

                    if (currentChatters.IsSuccessStatusCode)
                    {
                        var chattersModel =
                            JsonConvert.DeserializeObject<TmiChattersIntermediate>(currentChatters.Content
                                .ReadAsStringAsync().Result);

                        _giveViewershipBytesCommand.Give(chattersModel.ChattersIntermediate);
                    }
                    else
                    {
                        _logger.LogError("Could not retrieve Chatters JSON from TMI service");
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Couldn't access the TMI Service");
            }
        }
    }
}