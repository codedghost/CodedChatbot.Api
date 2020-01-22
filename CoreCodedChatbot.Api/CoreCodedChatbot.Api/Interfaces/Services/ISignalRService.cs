using Microsoft.AspNetCore.SignalR.Client;

namespace CoreCodedChatbot.Api.Interfaces.Services
{
    public interface ISignalRService
    {
        HubConnection GetCurrentConnection();
    }
}