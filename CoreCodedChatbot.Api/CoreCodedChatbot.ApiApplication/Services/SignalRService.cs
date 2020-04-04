using System;
using CoreCodedChatbot.ApiApplication.Interfaces.Services;
using CoreCodedChatbot.Config;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;

namespace CoreCodedChatbot.ApiApplication.Services
{
    public class SignalRService : ISignalRService
    {
        private readonly HubConnection _connection;

        public SignalRService(
            IConfigService configService,
            ILogger<SignalRService> logger
            )
        {
            try
            {
                _connection = new HubConnectionBuilder()
                    .WithUrl($"{configService.Get<string>("WebPlaylistUrl")}/SongList")
                    .Build();

                _connection.StartAsync().Wait();
            }
            catch (Exception e)
            {
                logger.LogError("Error when creating SignalR Connection", e);
                _connection = null;
            }
        }

        public HubConnection GetCurrentConnection()
        {
            return _connection;
        }
    }
}