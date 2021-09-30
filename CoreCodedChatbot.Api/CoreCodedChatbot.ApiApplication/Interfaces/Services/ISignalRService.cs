using System.Collections.Generic;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiContract.Enums.Playlist;
using CoreCodedChatbot.ApiContract.SignalRHubModels.Website.ClientSpecific;
using Microsoft.AspNetCore.SignalR.Client;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Services
{
    public interface ISignalRService
    {
        HubConnection GetCurrentConnection(string hubEndpoint);
        Task UpdatePlaylistState(PlaylistState state);
        Task UpdateVips(string clientId, int totalVips);
        Task UpdateBytes(IEnumerable<string> clientIds, string totalBytes);
    }
}