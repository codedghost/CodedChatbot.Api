using System;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Repositories.ChannelRewards
{
    public interface IStoreChannelRewardRedemptionRepository
    {
        void Store(Guid channelRewardId, string redeemedBy);
    }
}