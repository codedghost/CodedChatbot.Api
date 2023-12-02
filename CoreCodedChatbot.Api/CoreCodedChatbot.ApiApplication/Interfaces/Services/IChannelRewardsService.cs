using System;
using System.Threading.Tasks;
using CommandTypes = CoreCodedChatbot.ApiContract.Enums.ChannelRewards.CommandTypes;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Services;

public interface IChannelRewardsService
{
    Task CreateOrUpdate(Guid rewardId, string rewardTitle, string rewardDescription);
    Task<CommandTypes> Store(Guid channelRewardId, string redeemedBy, Guid channelRewardsRedemptionId);
    void Initialise();
}