using System;
using System.Linq;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Repositories.Abstractions;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;

namespace CoreCodedChatbot.ApiApplication.Repositories.Ylyl;

public class YlylRewardsRepository : BaseRepository<YlylReward>
{
    private readonly Random _rand;

    public YlylRewardsRepository(IChatbotContextFactory chatbotContextFactory) : base(chatbotContextFactory)
    {
        _rand = new Random();
    }

    public YlylReward GetRandomReward()
    {
        var activeRewards = GetAll()
            .Where(r => !r.IsRedeemed);

        var randNum = _rand.Next(activeRewards.Count());

        return activeRewards.ToArray()[randNum];
    }

    public async Task RedeemRewardAsync(int rewardId, int entryId)
    {
        var reward = await GetByIdAsync(rewardId);

        reward.YlylEntryId = entryId;
        reward.IsRedeemed = true;

        await Context.SaveChangesAsync();
    }
}