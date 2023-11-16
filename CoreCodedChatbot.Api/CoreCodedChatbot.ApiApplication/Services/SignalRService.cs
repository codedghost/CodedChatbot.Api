using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Interfaces.Services;
using CoreCodedChatbot.ApiContract.Enums.Playlist;
using CoreCodedChatbot.ApiContract.SignalRHubModels.API;
using CoreCodedChatbot.ApiContract.SignalRHubModels.Website;
using CoreCodedChatbot.ApiContract.SignalRHubModels.Website.ClientSpecific;
using CoreCodedChatbot.Config;
using CoreCodedChatbot.Secrets;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;

namespace CoreCodedChatbot.ApiApplication.Services;

public class SignalRService : ISignalRService
{
    private readonly IConfigService _configService;
    private readonly ISecretService _secretService;
    private readonly ILogger<SignalRService> _logger;
    private Dictionary<string, HubConnection> _hubConnections;

    public SignalRService(
        IConfigService configService,
        ISecretService secretService,
        ILogger<SignalRService> logger
    )
    {
        _configService = configService;
        _secretService = secretService;
        _logger = logger;

        _hubConnections = new Dictionary<string, HubConnection>
        {
            {
                APIHubConstants.SongListHubPath, Connect(APIHubConstants.SongListHubPath)
            }
        };
    }

    private HubConnection Connect(string hubEndpoint)
    {
        HubConnection connection;
        try
        {
            connection = new HubConnectionBuilder()
                .WithUrl($"{_configService.Get<string>("WebPlaylistUrl")}{hubEndpoint}")
                .Build();

            connection.StartAsync().Wait();

            connection.On("Heartbeat", () => { Console.WriteLine("heartbeat ping"); });
        }
        catch (Exception e)
        {
            _logger.LogError("Error when creating SignalR Connection", e);
            connection = null;
        }

        return connection;
    }

    public HubConnection GetCurrentConnection(string hubEndpoint)
    {
        if (_hubConnections[hubEndpoint] == null || _hubConnections[hubEndpoint].State != HubConnectionState.Connected)
        {
            _hubConnections[hubEndpoint] = Connect(hubEndpoint);
        }

        return _hubConnections[hubEndpoint];
    }

    public Task UpdatePlaylistState(PlaylistState state)
    {
        var connection = GetCurrentConnection(APIHubConstants.SongListHubPath);

        if (connection == null) return Task.CompletedTask;

        var psk = _secretService.GetSecret<string>("SignalRKey");

        return connection.InvokeAsync("PlaylistState", new PlaylistStateUpdateModel
        {
            psk = psk,
            playlistState = state.DisplayString()
        });
    }

    public Task UpdateVips(string clientId, int totalVips)
    {
        var connection = GetCurrentConnection(APIHubConstants.SongListHubPath);

        if (connection == null) return Task.CompletedTask;

        var psk = _secretService.GetSecret<string>("SignalRKey");

        return connection.InvokeAsync("UpdateVips", new VipTotalUpdateModel
        {
            ClientId = clientId,
            psk = psk,
            VipTotal = totalVips
        });
    }

    public Task UpdateBytes(IEnumerable<string> clientIds, string totalBytes)
    {
        var connection = GetCurrentConnection(APIHubConstants.SongListHubPath);

        if (connection == null) return Task.CompletedTask;

        var psk = _secretService.GetSecret<string>("SignalRKey");

        return connection.InvokeAsync("UpdateBytes", new ByteTotalUpdateModel
        {
            ClientIds = clientIds,
            TotalBytes = totalBytes,
            psk = psk
        });
    }
}