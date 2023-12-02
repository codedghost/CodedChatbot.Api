using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Services;

public interface IClientIdService
{
    Task SaveClientId(string hubType, string clientId, string username);
    Task RemoveClientId(string hubType, string clientId);
    Task<List<string>> GetClientIds(string username, string hubType);
}