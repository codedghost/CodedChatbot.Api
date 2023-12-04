using System;
using System.Net.Http.Headers;
using System.Text;
using CodedGhost.RabbitMQTools;
using CoreCodedChatbot.ApiApplication.Factories;
using CoreCodedChatbot.ApiApplication.Interfaces.Factories;
using CoreCodedChatbot.ApiApplication.Interfaces.Services;
using CoreCodedChatbot.ApiApplication.Models.Solr;
using CoreCodedChatbot.ApiApplication.Services;
using CoreCodedChatbot.Secrets;
using Microsoft.Extensions.DependencyInjection;
using PrintfulLib.ExternalClients;
using PrintfulLib.Interfaces.ExternalClients;
using SolrNet;

namespace CoreCodedChatbot.ApiApplication;

public static class Package
{
    public static IServiceCollection AddFactories(this IServiceCollection services)
    {
        services.AddSingleton<IPrintfulWebhookSetupFactory, PrintfulWebhookSetupFactory>();

        return services;
    }

    public static IServiceCollection ConfigureSolr(this IServiceCollection services, ISecretService secretService)
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

    public static IServiceCollection AddAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        return services;
    }

    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddSingleton<IAzureDevOpsService, AzureDevOpsService>();
        services.AddSingleton<IChannelRewardsService, ChannelRewardsService>();
        services.AddSingleton<IChatCommandService, ChatCommandService>();
        services.AddSingleton<IChatService, ChatService>();
        services.AddSingleton<IClientIdService, ClientIdService>();
        services.AddSingleton<IClientTriggerService, ClientTriggerService>();
        services.AddTransient<ICounterService, CounterService>();
        services.AddSingleton<IDownloadChartService, DownloadChartService>();
        services.AddSingleton<IGuessingGameService, GuessingGameService>();
        services.AddSingleton<IModerationService, ModerationService>();
        services.AddSingleton<IPlaylistService, PlaylistService>();
        services.AddTransient<IQuoteService, QuoteService>();
        services.AddSingleton<ISearchService, SearchService>();
        services.AddSingleton<ISettingsService, SettingsService>();
        services.AddSingleton<ISignalRService, SignalRService>();
        services.AddTransient<ISolrService, SolrService>();
        services.AddSingleton<IStreamLabsService, StreamLabsService>();
        services.AddSingleton<IVipService, VipService>();
        services.AddSingleton<IWatchTimeService, WatchTimeService>();

        return services;
    }
}