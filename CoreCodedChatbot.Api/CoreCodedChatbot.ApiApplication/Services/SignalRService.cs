using System;
using CoreCodedChatbot.ApiApplication.Interfaces.Services;
using CoreCodedChatbot.Config;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;

namespace CoreCodedChatbot.ApiApplication.Services
{
    public class SignalRService : ISignalRService
    {
        private readonly IConfigService _configService;
        private readonly ILogger<SignalRService> _logger;
        private HubConnection _connection;

        public SignalRService(
            IConfigService configService,
            ILogger<SignalRService> logger
            )
        {
            _configService = configService;
            _logger = logger;

            Connect();
        }

        private void Connect()
        {
            try
            {
                _connection = new HubConnectionBuilder()
                    .WithUrl($"{_configService.Get<string>("WebPlaylistUrl")}/SongList")
                    .Build();

                _connection.StartAsync().Wait();
            }
            catch (Exception e)
            {
                _logger.LogError("Error when creating SignalR Connection", e);
                _connection = null;
            }

        }

        public HubConnection GetCurrentConnection()
        {
            if (_connection == null)
                Connect();

            return _connection;
        }
    }
}