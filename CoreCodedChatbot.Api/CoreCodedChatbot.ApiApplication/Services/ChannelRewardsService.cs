using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CodedChatbot.TwitchFactories.Interfaces;
using CoreCodedChatbot.ApiApplication.Interfaces.Services;
using CoreCodedChatbot.ApiApplication.Repositories.ChannelRewards;
using CoreCodedChatbot.Database.Context.Enums;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;
using TwitchLib.Api;

namespace CoreCodedChatbot.ApiApplication.Services;

public class ChannelRewardsService : IBaseService, IChannelRewardsService
{
    private readonly IChatbotContextFactory _chatbotContextFactory;
    private readonly IVipService _vipService;

    private Timer _updateChannelRewardsTimer;
    private readonly TwitchAPI _twitchApi;

    public ChannelRewardsService(
        IChatbotContextFactory chatbotContextFactory,
        ITwitchApiFactory twitchApiFactory,
        IVipService vipService
    )
    {
        _chatbotContextFactory = chatbotContextFactory;
        _vipService = vipService;

        _twitchApi = twitchApiFactory.Get();
    }

    public void Initialise()
    {
        _updateChannelRewardsTimer = new Timer(e =>
            {
                UpdateChannelRewards().Wait();
                ProcessUnprocessedRedemptions().Wait();
            },
            null,
            TimeSpan.Zero,
            TimeSpan.FromHours(1));
    }

    private async Task UpdateChannelRewards()
    {
        var loggedInUser = await _twitchApi.Helix.Users.GetUsersAsync();
        var channelRewardsRequest =
            await _twitchApi.Helix.ChannelPoints.GetCustomRewardAsync(loggedInUser.Users.FirstOrDefault()?.Id);

        if (channelRewardsRequest == null) return;

        foreach (var reward in channelRewardsRequest.Data)
        {
            await CreateOrUpdate(Guid.Parse(reward.Id), reward.Title, reward.Prompt);
        }
    }

    public async Task CreateOrUpdate(Guid rewardId, string rewardTitle, string rewardDescription)
    {
        using (var repo = new ChannelRewardsRepository(_chatbotContextFactory))
        {
            await repo.CreateOrUpdate(rewardId, rewardTitle, rewardDescription);
        }
    }

    public async Task<ApiContract.Enums.ChannelRewards.CommandTypes> Store(Guid channelRewardId, string redeemedBy, Guid channelRewardsRedemptionId)
    {
        ChannelReward? channelReward;

        using (var repo = new ChannelRewardsRepository(_chatbotContextFactory))
        {
            channelReward = await repo.GetByIdOrNullAsync(channelRewardId);
        }

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

        using (var repo = new ChannelRewardRedemptionsRepository(_chatbotContextFactory))
        {
            await repo.Store(channelRewardsRedemptionId, channelRewardId, redeemedBy, processed);
        }
        
        return Enum.Parse<ApiContract.Enums.ChannelRewards.CommandTypes>(channelReward.CommandType.ToString());
    }

    public async Task ProcessUnprocessedRedemptions()
    {
        IEnumerable<ChannelRewardRedemption> unprocessedRedemptions;
        using (var repo = new ChannelRewardRedemptionsRepository(_chatbotContextFactory))
        {
            unprocessedRedemptions = await repo.Get(false);
        }

        foreach (var redemption in unprocessedRedemptions)
        {
            await Store(redemption.ChannelRewardId, redemption.Username, redemption.ChannelRewardRedemptionId);
        }
    }
}