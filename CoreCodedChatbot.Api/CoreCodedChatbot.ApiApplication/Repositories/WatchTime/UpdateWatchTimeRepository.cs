using System.Collections.Generic;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Repositories.Abstractions;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;
using CoreCodedChatbot.Database.DbExtensions;

namespace CoreCodedChatbot.ApiApplication.Repositories.WatchTime;

public class UpdateWatchTimeRepository : BaseRepository<User>
{
    public UpdateWatchTimeRepository(IChatbotContextFactory chatbotContextFactory)
        : base(chatbotContextFactory)
    {
    }

    public async Task Update(IEnumerable<string> chatters)
    {
        foreach (var username in chatters)
        {
            var user = Context.GetOrCreateUser(username);

            user.WatchTime++;
        }

        await Context.SaveChangesAsync();
    }
}