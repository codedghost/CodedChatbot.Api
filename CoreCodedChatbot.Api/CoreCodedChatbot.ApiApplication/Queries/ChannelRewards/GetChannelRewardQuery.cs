using System;
using CoreCodedChatbot.ApiApplication.Interfaces.Queries.ChannelRewards;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.ChannelRewards;
using CoreCodedChatbot.Database.Context.Models;

namespace CoreCodedChatbot.ApiApplication.Queries.ChannelRewards
{
    public class GetChannelRewardQuery : IGetChannelRewardQuery
    {
        private readonly IGetChannelRewardRepository _getChannelRewardRepository;

        public GetChannelRewardQuery(IGetChannelRewardRepository getChannelRewardRepository)
        {
            _getChannelRewardRepository = getChannelRewardRepository;
        }

        public ChannelReward GetChannelReward(Guid channelId)
        {
            return _getChannelRewardRepository.GetById(channelId);
        }
    }
}