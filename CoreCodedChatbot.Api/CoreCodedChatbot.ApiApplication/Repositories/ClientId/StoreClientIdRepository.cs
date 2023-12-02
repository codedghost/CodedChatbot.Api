using System.Collections.Generic;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Repositories.Abstractions;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;
using CoreCodedChatbot.Database.DbExtensions;

namespace CoreCodedChatbot.ApiApplication.Repositories.ClientId;

public class StoreClientIdRepository : BaseRepository<User>
{
    public StoreClientIdRepository(IChatbotContextFactory chatbotContextFactory)
        : base(chatbotContextFactory)
    {
    }

    public async Task Store(string hubType, string username, string clientId)
    {
        var user = Context.GetOrCreateUser(username);

        var clientIds = user.GetClientIdsDictionary();

        if (!clientIds.ContainsKey(hubType))
        {
            clientIds.Add(hubType, new List<string>());
        }

        clientIds[hubType].Add(clientId);

        user.UpdateClientIdsDictionary(clientIds);

        await Context.SaveChangesAsync();
    }
}