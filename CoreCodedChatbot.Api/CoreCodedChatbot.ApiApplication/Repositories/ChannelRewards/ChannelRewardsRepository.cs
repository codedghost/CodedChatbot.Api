using CoreCodedChatbot.ApiApplication.Repositories.Abstractions;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;
using System.Threading.Tasks;
using System;

namespace CoreCodedChatbot.ApiApplication.Repositories.ChannelRewards;

public class ChannelRewardsRepository : BaseRepository<ChannelReward>
{
    public ChannelRewardsRepository(IChatbotContextFactory chatbotContextFactory) : base(chatbotContextFactory)
    {
    }

    public async Task CreateOrUpdate(Guid rewardId, string rewardTitle, string rewardDescription)
    {
        var reward = await GetByIdOrNullAsync(rewardId);

        if (reward == null)
        {
            reward = new ChannelReward
            {
                ChannelRewardId = rewardId
            };

            await CreateAsync(reward);
        }

        reward.RewardTitle = rewardTitle;
        reward.RewardDescription = rewardDescription;

        await Context.SaveChangesAsync();
    }
}