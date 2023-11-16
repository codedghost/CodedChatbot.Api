using System.Collections.Generic;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.WatchTime;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.DbExtensions;

namespace CoreCodedChatbot.ApiApplication.Repositories.WatchTime;

public class UpdateWatchTimeRepository : IUpdateWatchTimeRepository
{
    private readonly IChatbotContextFactory _chatbotContextFactory;

    public UpdateWatchTimeRepository(IChatbotContextFactory chatbotContextFactory)
    {
        _chatbotContextFactory = chatbotContextFactory;
    }

    public async Task Update(IEnumerable<string> chatters)
    {
        using (var context = _chatbotContextFactory.Create())
        {
            foreach (var username in chatters)
            {
                var user = context.GetOrCreateUser(username);

                user.WatchTime++;
            }

            await context.SaveChangesAsync();
        }
    }
}