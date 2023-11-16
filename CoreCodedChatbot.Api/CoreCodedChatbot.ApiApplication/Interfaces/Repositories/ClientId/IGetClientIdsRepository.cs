using System.Collections.Generic;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Repositories.ClientId;

public interface IGetClientIdsRepository
{
    List<string> Get(string username, string hubType);
}