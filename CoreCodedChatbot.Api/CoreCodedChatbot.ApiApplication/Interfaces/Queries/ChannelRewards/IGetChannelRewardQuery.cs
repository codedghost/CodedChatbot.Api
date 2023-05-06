using System;
using CoreCodedChatbot.Database.Context.Models;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Queries.ChannelRewards
{
    public interface IGetChannelRewardQuery
    {
        ChannelReward GetChannelReward(Guid channelId);
    }
}