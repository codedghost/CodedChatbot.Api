using System;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Repositories.Abstractions;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;

namespace CoreCodedChatbot.ApiApplication.Repositories.Vip;

public class UseSuperVipRepository : BaseRepository<User>
{
    public UseSuperVipRepository(
        IChatbotContextFactory chatbotContextFactory
    ) : base(chatbotContextFactory)
    {
    }

    public async Task UseSuperVip(string username, int vipsToUse, int superVipsToRegister)
    {
        var user = await GetByIdOrNullAsync(username);

        if (user == null) throw new UnauthorizedAccessException("User does not exist");

        user.UsedSuperVipRequests += superVipsToRegister;
        user.UsedVipRequests += vipsToUse;

        await Context.SaveChangesAsync();
    }
}