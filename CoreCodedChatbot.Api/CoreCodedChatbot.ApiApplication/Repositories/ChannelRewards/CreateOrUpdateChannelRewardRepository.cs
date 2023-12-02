using System;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Repositories.Abstractions;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;

namespace CoreCodedChatbot.ApiApplication.Repositories.ChannelRewards;

public class CreateOrUpdateChannelRewardRepository : BaseRepository<ChannelReward>
{
    public CreateOrUpdateChannelRewardRepository(IChatbotContextFactory chatbotContextFactory)
        : base(chatbotContextFactory)
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