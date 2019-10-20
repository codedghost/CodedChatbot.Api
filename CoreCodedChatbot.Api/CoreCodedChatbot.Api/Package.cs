using CoreCodedChatbot.Library.Models.Data;
using Microsoft.Extensions.DependencyInjection;
using TwitchLib.Api;
using TwitchLib.Client;
using TwitchLib.Client.Models;

namespace CoreCodedChatbot.Api
{
    public static class Package
    {
        public static IServiceCollection AddTwitchServices(this IServiceCollection services, ConfigModel config)
        {
            var api = new TwitchAPI();
            api.Settings.AccessToken = config.ChatbotAccessToken;

            // TODO: Remove the need for the playlist service to talk directly in chat when opening the playlist.
            var creds = new ConnectionCredentials(config.ChatbotNick, config.ChatbotPass);
            var client = new TwitchClient();
            client.Initialize(creds, config.StreamerChannel);
            client.Connect();

            services.AddSingleton(api);
            services.AddSingleton(client);

            return services;
        }
    }
}