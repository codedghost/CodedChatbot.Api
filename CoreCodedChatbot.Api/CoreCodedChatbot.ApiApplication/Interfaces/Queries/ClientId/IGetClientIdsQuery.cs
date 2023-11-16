using System.Collections.Generic;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Queries.ClientId;

public interface IGetClientIdsQuery
{
    List<string> Get(string username, string hubType);
}