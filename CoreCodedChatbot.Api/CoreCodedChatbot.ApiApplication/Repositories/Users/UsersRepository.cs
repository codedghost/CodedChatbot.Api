using CoreCodedChatbot.ApiApplication.Repositories.Abstractions;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;
using CoreCodedChatbot.Database.DbExtensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreCodedChatbot.ApiApplication.Repositories.Users;

public class UsersRepository : BaseRepository<User>
{
    public UsersRepository(IChatbotContextFactory chatbotContextFactory) : base(chatbotContextFactory)
    {
    }


    #region Bytes

    #endregion

    #region WatchTime

    public async Task<TimeSpan> GetWatchTime(string username)
    {
        var user = await GetByIdOrNullAsync(username);

        return user == null ? TimeSpan.Zero : TimeSpan.FromMinutes(user.WatchTime);
    }

    public async Task UpdateWatchTime(IEnumerable<string> chatters)
    {
        foreach (var username in chatters)
        {
            var user = Context.GetOrCreateUser(username);

            user.WatchTime++;
            user.TimeLastInChat = DateTime.UtcNow;
        }

        await Context.SaveChangesAsync();
    }

    #endregion
}