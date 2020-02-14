using CoreCodedChatbot.ApiApplication.Interfaces.Services;
using CoreCodedChatbot.Config;
using Microsoft.AspNetCore.SignalR.Client;

namespace CoreCodedChatbot.ApiApplication.Services
{
    public class SignalRService : ISignalRService
    {
        private readonly HubConnection _connection;

        public SignalRService(
            IConfigService configService
            )
        {
            _connection = new HubConnectionBuilder()
                .WithUrl($"{configService.Get<string>("WebPlaylistUrl")}/SongList")
                .Build();

            _connection.StartAsync().Wait();
        }

        public HubConnection GetCurrentConnection()
        {
            return _connection;
        }
    }
}