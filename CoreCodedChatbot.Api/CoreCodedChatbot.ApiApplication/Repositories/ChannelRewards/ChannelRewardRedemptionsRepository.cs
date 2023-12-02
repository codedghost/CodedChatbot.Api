using CoreCodedChatbot.ApiApplication.Repositories.Abstractions;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;

namespace CoreCodedChatbot.ApiApplication.Repositories.ChannelRewards;

public class ChannelRewardRedemptionsRepository : BaseRepository<ChannelRewardRedemption>
{
    public ChannelRewardRedemptionsRepository(IChatbotContextFactory chatbotContextFactory) : base(chatbotContextFactory)
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

    public async Task Store(Guid channelRewardsRedemptionId, Guid channelRewardId, string redeemedBy, bool processed)
    {
        var redemption = await GetByIdOrNullAsync(channelRewardsRedemptionId);

        if (redemption == null)
        {
            redemption = new ChannelRewardRedemption
            {
                ChannelRewardId = channelRewardId,
                Username = redeemedBy,
                RedeemedAt = DateTime.Now
            };

            await CreateAsync(redemption);
        }

        redemption.Processed = processed;

        await Context.SaveChangesAsync();
    }
}