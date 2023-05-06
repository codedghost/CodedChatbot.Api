using System;
using CoreCodedChatbot.Database.Context.Models;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Repositories.ChannelRewards;

public interface IGetChannelRewardRepository
{
    ChannelReward GetById(Guid id);
}