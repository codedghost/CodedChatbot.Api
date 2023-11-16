using System;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.ChannelRewards;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.ChannelRewards;

namespace CoreCodedChatbot.ApiApplication.Commands.ChannelRewards;

public class CreateOrUpdateChannelRewardCommand : ICreateOrUpdateChannelRewardCommand
{
    private readonly ICreateOrUpdateChannelRewardRepository _createOrUpdateChannelRewardRepository;

    public CreateOrUpdateChannelRewardCommand(ICreateOrUpdateChannelRewardRepository createOrUpdateChannelRewardRepository)
    {
        _createOrUpdateChannelRewardRepository = createOrUpdateChannelRewardRepository;
    }

    public void CreateOrUpdate(Guid rewardId, string rewardTitle, string rewardDescription)
    {
        _createOrUpdateChannelRewardRepository.CreateOrUpdate(rewardId, rewardTitle, rewardDescription);
    }
}