using System;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Repositories.ChannelRewards;

public interface IStoreChannelRewardRedemptionRepository
{
    void Store(Guid channelRewardsRedemptionId, Guid channelRewardId, string redeemedBy, bool processed);
}