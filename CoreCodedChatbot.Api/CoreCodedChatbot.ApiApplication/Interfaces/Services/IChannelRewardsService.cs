using System;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Services
{
    public interface IChannelRewardsService
    {
        void CreateOrUpdate(Guid rewardId, string rewardTitle, string rewardDescription);
        void Store(Guid channelRewardId, string redeemedBy);
        void Initialise();
    }
}