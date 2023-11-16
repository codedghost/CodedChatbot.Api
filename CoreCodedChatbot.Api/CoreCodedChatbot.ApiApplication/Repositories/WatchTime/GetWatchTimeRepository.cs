using System;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.WatchTime;
using CoreCodedChatbot.Database.Context.Interfaces;

namespace CoreCodedChatbot.ApiApplication.Repositories.WatchTime;

public class GetWatchTimeRepository : IGetWatchTimeRepository
{
    private readonly IChatbotContextFactory _chatbotContextFactory;

    public GetWatchTimeRepository(IChatbotContextFactory chatbotContextFactory)
    {
        _chatbotContextFactory = chatbotContextFactory;
    }

    public async Task<TimeSpan> Get(string username)
    {
        using (var context = _chatbotContextFactory.Create())
        {
            var user = await context.Users.FindAsync(username);

            return user == null ? TimeSpan.Zero : TimeSpan.FromMinutes(user.WatchTime);
        }
    }
}