using System;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.ChannelRewards;
using CoreCodedChatbot.ApiApplication.Interfaces.Services;

namespace CoreCodedChatbot.ApiApplication.Services
{
    public class ChannelRewardsService : IChannelRewardsService
    {
        private readonly ICreateOrUpdateChannelRewardCommand _createOrUpdateChannelRewardCommand;
        private readonly IStoreChannelRewardRedemptionCommand _storeChannelRewardRedemptionCommand;

        public ChannelRewardsService(
            ICreateOrUpdateChannelRewardCommand createOrUpdateChannelRewardCommand,
            IStoreChannelRewardRedemptionCommand storeChannelRewardRedemptionCommand
            )
        {
            _createOrUpdateChannelRewardCommand = createOrUpdateChannelRewardCommand;
            _storeChannelRewardRedemptionCommand = storeChannelRewardRedemptionCommand;
        }

        public void CreateOrUpdate(Guid rewardId, string rewardTitle, string rewardDescription)
        {
            _createOrUpdateChannelRewardCommand.CreateOrUpdate(rewardId, rewardTitle, rewardDescription);
        }

        public void Store(Guid channelRewardId, string redeemedBy)
        {
            _storeChannelRewardRedemptionCommand.Store(channelRewardId, redeemedBy);
        }
    }
}