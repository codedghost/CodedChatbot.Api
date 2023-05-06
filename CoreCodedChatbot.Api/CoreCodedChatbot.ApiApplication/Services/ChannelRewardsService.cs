﻿using System;
using System.Linq;
using System.Threading;
using CodedChatbot.TwitchFactories.Interfaces;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.ChannelRewards;
using CoreCodedChatbot.ApiApplication.Interfaces.Services;
using TwitchLib.Api;

namespace CoreCodedChatbot.ApiApplication.Services
{
    public class ChannelRewardsService : IChannelRewardsService
    {
        private readonly ICreateOrUpdateChannelRewardCommand _createOrUpdateChannelRewardCommand;
        private readonly IStoreChannelRewardRedemptionCommand _storeChannelRewardRedemptionCommand;

        private Timer _updateChannelRewardsTimer;
        private readonly TwitchAPI _twitchApi;

        public ChannelRewardsService(
            ICreateOrUpdateChannelRewardCommand createOrUpdateChannelRewardCommand,
            IStoreChannelRewardRedemptionCommand storeChannelRewardRedemptionCommand,
            ITwitchApiFactory twitchApiFactory
            )
        {
            _createOrUpdateChannelRewardCommand = createOrUpdateChannelRewardCommand;
            _storeChannelRewardRedemptionCommand = storeChannelRewardRedemptionCommand;

            _twitchApi = twitchApiFactory.Get();
        }

        public void Initialise()
        {
            _updateChannelRewardsTimer = new Timer(e => { UpdateChannelRewards(); },
                null,
                TimeSpan.Zero,
                TimeSpan.FromHours(1));
        }

        private async void UpdateChannelRewards()
        {
            var loggedInUser = await _twitchApi.Helix.Users.GetUsersAsync();
            var channelRewardsRequest =
                await _twitchApi.Helix.ChannelPoints.GetCustomRewardAsync(loggedInUser.Users.FirstOrDefault()?.Id);

            if (channelRewardsRequest == null) return;

            foreach (var reward in channelRewardsRequest.Data)
            {
                CreateOrUpdate(Guid.Parse(reward.Id), reward.Title, reward.Prompt);
            }
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