﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CodedChatbot.TwitchFactories.Interfaces;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.ChannelRewards;
using CoreCodedChatbot.ApiApplication.Interfaces.Queries.ChannelRewards;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.ChannelRewards;
using CoreCodedChatbot.ApiApplication.Interfaces.Services;
using CoreCodedChatbot.Database.Context.Enums;
using TwitchLib.Api;

namespace CoreCodedChatbot.ApiApplication.Services
{
    public class ChannelRewardsService : IChannelRewardsService
    {
        private readonly ICreateOrUpdateChannelRewardCommand _createOrUpdateChannelRewardCommand;
        private readonly IStoreChannelRewardRedemptionCommand _storeChannelRewardRedemptionCommand;
        private readonly IGetChannelRewardQuery _getChannelRewardQuery;
        private readonly IGetChannelRewardRedemptionsRepository _getChannelRewardRedemptionsRepository;
        private readonly IVipService _vipService;

        private Timer _updateChannelRewardsTimer;
        private readonly TwitchAPI _twitchApi;

        public ChannelRewardsService(
            ICreateOrUpdateChannelRewardCommand createOrUpdateChannelRewardCommand,
            IStoreChannelRewardRedemptionCommand storeChannelRewardRedemptionCommand,
            IGetChannelRewardQuery getChannelRewardQuery,
            IGetChannelRewardRedemptionsRepository getChannelRewardRedemptionsRepository,
            ITwitchApiFactory twitchApiFactory,
            IVipService vipService
        )
        {
            _createOrUpdateChannelRewardCommand = createOrUpdateChannelRewardCommand;
            _storeChannelRewardRedemptionCommand = storeChannelRewardRedemptionCommand;
            _getChannelRewardQuery = getChannelRewardQuery;
            _getChannelRewardRedemptionsRepository = getChannelRewardRedemptionsRepository;
            _vipService = vipService;

            _twitchApi = twitchApiFactory.Get();
        }

        public void Initialise()
        {
            _updateChannelRewardsTimer = new Timer(e =>
                {
                    UpdateChannelRewards();
                    ProcessUnprocessedRedemptions().Wait();
                },
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

        public ApiContract.Enums.ChannelRewards.CommandTypes Store(Guid channelRewardId, string redeemedBy, Guid channelRewardsRedemptionId)
        {
            var channelReward = _getChannelRewardQuery.GetChannelReward(channelRewardId);

            if (channelReward == null) return ApiContract.Enums.ChannelRewards.CommandTypes.None;
            var processed = false;

            switch (channelReward.CommandType)
            {
                case CommandTypes.None:
                    break;
                case CommandTypes.ConvertToVip:
                    _vipService.GiveChannelPointsVip(redeemedBy);
                    processed = true;
                    break;
                default:
                    return ApiContract.Enums.ChannelRewards.CommandTypes.None;
            }

            _storeChannelRewardRedemptionCommand.Store(channelRewardsRedemptionId, channelRewardId, redeemedBy, processed);

            return Enum.Parse<ApiContract.Enums.ChannelRewards.CommandTypes>(channelReward.CommandType.ToString());
        }

        public async Task ProcessUnprocessedRedemptions()
        {
            var unprocessedRedemptions = await _getChannelRewardRedemptionsRepository.Get(false).ConfigureAwait(false);

            foreach (var redemption in unprocessedRedemptions)
            {
                Store(redemption.ChannelRewardId, redemption.Username, redemption.ChannelRewardRedemptionId);
            }
        }
    }
}