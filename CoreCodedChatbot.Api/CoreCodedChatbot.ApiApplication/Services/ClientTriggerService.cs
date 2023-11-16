using CodedChatbot.TwitchFactories.Interfaces;
using CoreCodedChatbot.ApiApplication.Hubs;
using CoreCodedChatbot.ApiApplication.Interfaces.Services;
using CoreCodedChatbot.Config;
using Microsoft.AspNetCore.SignalR;

namespace CoreCodedChatbot.ApiApplication.Services;

public class ClientTriggerService : IClientTriggerService
{
    private readonly IHubContext<BackgroundSongHub> _backgroundSongHubContext;
    private readonly IConfigService _configService;
    private readonly ITwitchClientFactory _twitchClientFactory;

    public ClientTriggerService(
        IHubContext<BackgroundSongHub> backgroundSongHubContext,
        IConfigService configService,
        ITwitchClientFactory twitchClientFactory)
    {
        _backgroundSongHubContext = backgroundSongHubContext;
        _configService = configService;
        _twitchClientFactory = twitchClientFactory;
    }

    public async void TriggerSongCheck(string username)
    {
        await _backgroundSongHubContext.Clients.All.SendAsync("BackgroundSongCheck", username);
    }

    public void SendSongToChat(string username, string title, string artist, string url)
    {
        var streamerChannelName = _configService.Get<string>("StreamerChannel");
        var twitchClient = _twitchClientFactory.Get();

        twitchClient.SendMessage(streamerChannelName,
            $"Hey @{username}, the current song playing in the background is called: {title} by {artist}. You can find more info here: {url}");
    }
}