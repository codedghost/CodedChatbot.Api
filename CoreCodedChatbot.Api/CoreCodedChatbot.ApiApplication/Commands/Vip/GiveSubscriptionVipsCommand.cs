using System.Collections.Generic;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Vip;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Vip;
using CoreCodedChatbot.ApiContract.RequestModels.Vip.ChildModels;
using CoreCodedChatbot.Config;

namespace CoreCodedChatbot.ApiApplication.Commands.Vip;

public class GiveSubscriptionVipsCommand : IGiveSubscriptionVipsCommand
{
    private readonly IConfigService _configService;
    private readonly IGiveSubVipsRepository _giveSubVipsRepository;

    public GiveSubscriptionVipsCommand(
        IConfigService configService,
        IGiveSubVipsRepository giveSubVipsRepository
    )
    {
        _configService = configService;
        _giveSubVipsRepository = giveSubVipsRepository;
    }

    public void Give(List<UserSubDetail> userSubDetails)
    {
        var tier2ExtraVips = _configService.Get<int>("Tier2ExtraVip");
        var tier3ExtraVips = _configService.Get<int>("Tier3ExtraVip");

        _giveSubVipsRepository.Give(userSubDetails, tier2ExtraVips, tier3ExtraVips);
    }
}