using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.ChannelRewards;
using CoreCodedChatbot.ApiApplication.Repositories.Abstractions;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;
using Microsoft.EntityFrameworkCore;

namespace CoreCodedChatbot.ApiApplication.Repositories.ChannelRewards;

public class GetChannelRewardRedemptionsRepository : BaseRepository<ChannelRewardRedemption>
{
    public GetChannelRewardRedemptionsRepository(IChatbotContextFactory chatbotContextFactory)
        : base(chatbotContextFactory)
    {
    }

    public async Task<IEnumerable<ChannelRewardRedemption>> Get(bool includeNonCommandTypes)
    {
        if (includeNonCommandTypes)
        {
            return await GetAll()
                .Where(crr => !crr.Processed)
                .ToListAsync();
        }

        var commandRewards = await Context.ChannelRewards.Where(cr => cr.CommandType != 0)
            .Select(cr => cr.ChannelRewardId).ToListAsync();

        return await GetAll()
            .Where(crr => !crr.Processed && commandRewards.Contains(crr.ChannelRewardId))
            .ToListAsync();
    }
}