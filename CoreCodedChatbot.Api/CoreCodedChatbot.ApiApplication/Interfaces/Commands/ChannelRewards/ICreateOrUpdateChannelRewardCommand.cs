using System;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Commands.ChannelRewards;

public interface ICreateOrUpdateChannelRewardCommand
{
    void CreateOrUpdate(Guid rewardId, string rewardTitle, string rewardDescription);
}