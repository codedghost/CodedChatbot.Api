using System;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Repositories.Abstractions;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;

namespace CoreCodedChatbot.ApiApplication.Repositories.WatchTime;

public class GetWatchTimeRepository : BaseRepository<User>
{
    public GetWatchTimeRepository(IChatbotContextFactory chatbotContextFactory)
        : base(chatbotContextFactory)
    {
    }

    public async Task<TimeSpan> Get(string username)
    {
        var user = await GetByIdOrNullAsync(username);

        return user == null ? TimeSpan.Zero : TimeSpan.FromMinutes(user.WatchTime);
    }
}