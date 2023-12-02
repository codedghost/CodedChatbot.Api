using System;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Repositories.Abstractions;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;

namespace CoreCodedChatbot.ApiApplication.Repositories.ChannelRewards;

public class StoreChannelRewardRedemptionRepository : BaseRepository<ChannelRewardRedemption>
{
    public StoreChannelRewardRedemptionRepository(IChatbotContextFactory chatbotContextFactory)
        : base(chatbotContextFactory)
    {
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