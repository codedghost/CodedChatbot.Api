﻿using System;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Commands.ChannelRewards
{
    public interface IStoreChannelRewardRedemptionCommand
    {
        void Store(Guid channelRewardId, string redeemedBy);
    }
}