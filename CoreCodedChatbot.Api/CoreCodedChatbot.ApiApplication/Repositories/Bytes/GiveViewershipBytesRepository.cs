using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Repositories.Abstractions;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;
using CoreCodedChatbot.Database.DbExtensions;

namespace CoreCodedChatbot.ApiApplication.Repositories.Bytes;

public class GiveViewershipBytesRepository : BaseRepository<User>
{
    public GiveViewershipBytesRepository(IChatbotContextFactory chatbotContextFactory)
        : base(chatbotContextFactory)
    {
    }

    public async Task Give(List<string> usernames)
    {
        foreach (var username in usernames)
        {
            var user = Context.GetOrCreateUser(username, true);

            //user.TokenBytes++;
            user.TimeLastInChat = DateTime.UtcNow;
        }

        await Context.SaveChangesAsync();
    }
}