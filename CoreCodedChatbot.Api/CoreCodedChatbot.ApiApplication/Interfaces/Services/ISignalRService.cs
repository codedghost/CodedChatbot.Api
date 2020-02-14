using Microsoft.AspNetCore.SignalR.Client;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Services
{
    public interface ISignalRService
    {
        HubConnection GetCurrentConnection();
    }
}