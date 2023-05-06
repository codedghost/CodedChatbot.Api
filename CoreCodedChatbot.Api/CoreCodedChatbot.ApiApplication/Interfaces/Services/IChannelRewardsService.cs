using System;
using CommandTypes = CoreCodedChatbot.ApiContract.Enums.ChannelRewards.CommandTypes;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Services
{
    public interface IChannelRewardsService
    {
        void CreateOrUpdate(Guid rewardId, string rewardTitle, string rewardDescription);
        CommandTypes Store(Guid channelRewardId, string redeemedBy);
        void Initialise();
    }
}