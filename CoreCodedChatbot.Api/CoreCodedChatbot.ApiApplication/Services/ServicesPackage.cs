using CoreCodedChatbot.ApiApplication.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CoreCodedChatbot.ApiApplication.Services;

public static class ServicesPackage
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddSingleton<IAzureDevOpsService, AzureDevOpsService>();
        services.AddSingleton<IChannelRewardsService, ChannelRewardsService>();
        services.AddSingleton<IClientIdService, ClientIdService>();
        services.AddSingleton<IClientTriggerService, ClientTriggerService>();
        services.AddSingleton<IChatCommandService, ChatCommandService>();
        services.AddSingleton<IChatService, ChatService>();
        services.AddTransient<ICounterService, CounterService>();
        services.AddSingleton<IDownloadChartService, DownloadChartService>();
        services.AddSingleton<IGuessingGameService, GuessingGameService>();
        services.AddSingleton<IModerationService, ModerationService>();
        services.AddSingleton<IPlaylistService, PlaylistService>();
        services.AddTransient<IQuoteService, QuoteService>();
        services.AddSingleton<ISearchService, SearchService>();
        services.AddSingleton<ISettingsService, SettingsService>();
        services.AddSingleton<ISignalRService, SignalRService>();
        services.AddSingleton<IStreamLabsService, StreamLabsService>();
        services.AddSingleton<IVipService, VipService>();
        services.AddSingleton<IWatchTimeService, WatchTimeService>();

        return services;
    }
}