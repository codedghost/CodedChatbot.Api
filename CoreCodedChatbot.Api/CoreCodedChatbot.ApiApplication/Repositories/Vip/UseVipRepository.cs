using System;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Repositories.Abstractions;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;

namespace CoreCodedChatbot.ApiApplication.Repositories.Vip;

public class UseVipRepository : BaseRepository<User>
{
    public UseVipRepository(
        IChatbotContextFactory chatbotContextFactory
    ) : base(chatbotContextFactory)
    {
    }

    public async Task UseVip(string username, int vips)
    {
        var user = await GetByIdOrNullAsync(username);

        if (user == null) throw new UnauthorizedAccessException("User does not exist");

        user.UsedVipRequests += vips;

        await Context.SaveChangesAsync();
    }
}