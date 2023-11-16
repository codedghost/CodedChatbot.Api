using System;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Commands.ChannelRewards;

public interface IStoreChannelRewardRedemptionCommand
{
    void Store(Guid channelRewardsRedemptionId, Guid channelRewardId, string redeemedBy, bool processed);
}