using System.Collections.Generic;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Repositories.Abstractions;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;

namespace CoreCodedChatbot.ApiApplication.Repositories.ClientId;

public class GetClientIdsRepository : BaseRepository<User>
{
    public GetClientIdsRepository(IChatbotContextFactory chatbotContextFactory)
        : base(chatbotContextFactory)
    {
    }

    public async Task<List<string>> Get(string username, string hubType)
    {
        var user = await GetByIdOrNullAsync(username);

        if (user == null) return new List<string>();

        var clientIds = user.GetClientIdsDictionary();

        return clientIds.ContainsKey(hubType) ? clientIds[hubType] : new List<string>();
    }
}