using System.Collections.Generic;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Services;

public interface IClientIdService
{
    void SaveClientId(string hubType, string clientId, string username);
    void RemoveClientId(string hubType, string clientId);
    List<string> GetClientIds(string username, string hubType);
}