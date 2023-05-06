using System;
using System.Net.Http.Headers;
using System.Text;
using CodedGhost.RabbitMQTools;
using CoreCodedChatbot.ApiApplication.Commands.AzureDevOps;
using CoreCodedChatbot.ApiApplication.Commands.Bytes;
using CoreCodedChatbot.ApiApplication.Commands.ChannelRewards;
using CoreCodedChatbot.ApiApplication.Commands.ChatCommand;
using CoreCodedChatbot.ApiApplication.Commands.ClientId;
using CoreCodedChatbot.ApiApplication.Commands.GuessingGame;
using CoreCodedChatbot.ApiApplication.Commands.Moderation;
using CoreCodedChatbot.ApiApplication.Commands.Playlist;
using CoreCodedChatbot.ApiApplication.Commands.Quote;
using CoreCodedChatbot.ApiApplication.Commands.Settings;
using CoreCodedChatbot.ApiApplication.Commands.StreamStatus;
using CoreCodedChatbot.ApiApplication.Commands.Vip;
using CoreCodedChatbot.ApiApplication.Factories;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.AzureDevOps;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Bytes;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.ChannelRewards;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.ChatCommand;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.ClientId;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.GuessingGame;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Moderation;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Playlist;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Quote;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Search;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Settings;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.StreamStatus;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Vip;
using CoreCodedChatbot.ApiApplication.Interfaces.Factories;
using CoreCodedChatbot.ApiApplication.Interfaces.Queries.AzureDevOps;
using CoreCodedChatbot.ApiApplication.Interfaces.Queries.Bytes;
using CoreCodedChatbot.ApiApplication.Interfaces.Queries.ChannelRewards;
using CoreCodedChatbot.ApiApplication.Interfaces.Queries.ChatCommand;
using CoreCodedChatbot.ApiApplication.Interfaces.Queries.ClientId;
using CoreCodedChatbot.ApiApplication.Interfaces.Queries.GuessingGame;
using CoreCodedChatbot.ApiApplication.Interfaces.Queries.Playlist;
using CoreCodedChatbot.ApiApplication.Interfaces.Queries.Quote;
using CoreCodedChatbot.ApiApplication.Interfaces.Queries.Search;
using CoreCodedChatbot.ApiApplication.Interfaces.Queries.StreamLabs;
using CoreCodedChatbot.ApiApplication.Interfaces.Queries.StreamStatus;
using CoreCodedChatbot.ApiApplication.Interfaces.Queries.Vip;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Bytes;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.ChannelRewards;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.ChatCommand;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.ClientId;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.GuessingGame;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Moderation;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Playlist;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Quote;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Search;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Settings;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.StreamLabs;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.StreamStatus;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Vip;
using CoreCodedChatbot.ApiApplication.Interfaces.Services;
using CoreCodedChatbot.ApiApplication.Models.Solr;
using CoreCodedChatbot.ApiApplication.Queries.AzureDevOps;
using CoreCodedChatbot.ApiApplication.Queries.Bytes;
using CoreCodedChatbot.ApiApplication.Queries.ChannelRewards;
using CoreCodedChatbot.ApiApplication.Queries.ChatCommand;
using CoreCodedChatbot.ApiApplication.Queries.ClientId;
using CoreCodedChatbot.ApiApplication.Queries.GuessingGame;
using CoreCodedChatbot.ApiApplication.Queries.Playlist;
using CoreCodedChatbot.ApiApplication.Queries.Quote;
using CoreCodedChatbot.ApiApplication.Queries.Search;
using CoreCodedChatbot.ApiApplication.Queries.StreamLabs;
using CoreCodedChatbot.ApiApplication.Queries.StreamStatus;
using CoreCodedChatbot.ApiApplication.Queries.Vip;
using CoreCodedChatbot.ApiApplication.Repositories.Bytes;
using CoreCodedChatbot.ApiApplication.Repositories.ChannelRewards;
using CoreCodedChatbot.ApiApplication.Repositories.ChatCommand;
using CoreCodedChatbot.ApiApplication.Repositories.ClientId;
using CoreCodedChatbot.ApiApplication.Repositories.GuessingGame;
using CoreCodedChatbot.ApiApplication.Repositories.Moderation;
using CoreCodedChatbot.ApiApplication.Repositories.Playlist;
using CoreCodedChatbot.ApiApplication.Repositories.Quote;
using CoreCodedChatbot.ApiApplication.Repositories.Search;
using CoreCodedChatbot.ApiApplication.Repositories.Settings;
using CoreCodedChatbot.ApiApplication.Repositories.StreamLabs;
using CoreCodedChatbot.ApiApplication.Repositories.StreamStatus;
using CoreCodedChatbot.ApiApplication.Repositories.Vip;
using CoreCodedChatbot.ApiApplication.Services;
using CoreCodedChatbot.Secrets;
using Microsoft.Extensions.DependencyInjection;
using PrintfulLib.ExternalClients;
using PrintfulLib.Interfaces.ExternalClients;
using SolrNet;

namespace CoreCodedChatbot.ApiApplication
{
    public static class Package
    {
        public static IServiceCollection AddFactories(this IServiceCollection services)
        {
            services.AddSingleton<IPrintfulWebhookSetupFactory, PrintfulWebhookSetupFactory>();

            return services;
        }

        public static IServiceCollection AddSolr(this IServiceCollection services, ISecretService secretService)
        {
            var solrUser = secretService.GetSecret<string>("SolrUsername");
            var solrPass = secretService.GetSecret<string>("SolrPassword");
            var solrUrl = secretService.GetSecret<string>("SolrUrl");

            var credentials = Encoding.ASCII.GetBytes($"{solrUser}:{solrPass}");

            var credentialsBase64 = Convert.ToBase64String(credentials);

            services.AddSolrNet<SongSearch>($"{solrUrl}/SongSearch", options =>
            {
                options.HttpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Basic", credentialsBase64);
            });

            services.AddScoped<ISolrService, SolrService>();
            services.AddSingleton<IDownloadChartService, DownloadChartService>();

            return services;
        }

        public static IServiceCollection AddRabbitConnectionServices(this IServiceCollection services)
        {
            services.AddConnectionRabbitServices()
                .AddRabbitPublisherService();

            return services;
        }

        public static IServiceCollection AddPrintfulClient(this IServiceCollection services, ISecretService secretService)
        {
            var printfulClient = new PrintfulClient(secretService.GetSecret<string>("PrintfulAPIKey"));

            services.AddSingleton<IPrintfulClient>(printfulClient);

            return services;
        }
    }
}
