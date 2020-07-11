using System;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.ChannelRewards;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.ChannelRewards;

namespace CoreCodedChatbot.ApiApplication.Commands.ChannelRewards
{
    public class StoreChannelRewardRedemptionCommand : IStoreChannelRewardRedemptionCommand
    {
        private readonly IStoreChannelRewardRedemptionRepository _storeChannelRewardRedemptionRepository;

        public StoreChannelRewardRedemptionCommand(IStoreChannelRewardRedemptionRepository storeChannelRewardRedemptionRepository)
        {
            _storeChannelRewardRedemptionRepository = storeChannelRewardRedemptionRepository;
        }

        public void Store(Guid channelRewardId, string redeemedBy)
        {
            _storeChannelRewardRedemptionRepository.Store(channelRewardId, redeemedBy);
        }
    }
}