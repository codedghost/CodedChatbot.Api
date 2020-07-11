using System;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Repositories.ChannelRewards
{
    public interface ICreateOrUpdateChannelRewardRepository
    {
        void CreateOrUpdate(Guid rewardId, string rewardTitle, string rewardDescription);
    }
}