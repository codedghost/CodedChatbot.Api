﻿using CoreCodedChatbot.ApiApplication.Commands.Vip;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Vip
{
    public interface IGiveChannelPointsVipRepository
    {
        void GiveChannelPointsVip(string username);
    }
}