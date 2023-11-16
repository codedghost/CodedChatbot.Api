using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Vip;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Vip;
using CoreCodedChatbot.Config;

namespace CoreCodedChatbot.ApiApplication.Commands.Vip;

public class UseSuperVipCommand : IUseSuperVipCommand
{
    private readonly IUseSuperVipRepository _useSuperVipRepository;
    private readonly IConfigService _configService;

    public UseSuperVipCommand(
        IUseSuperVipRepository useSuperVipRepository,
        IConfigService configService
    )
    {
        _useSuperVipRepository = useSuperVipRepository;
        _configService = configService;
    }

    public void UseSuperVip(string username, int discount)
    {
        var vipsToUse = _configService.Get<int>("SuperVipCost") - discount;

        _useSuperVipRepository.UseSuperVip(username, vipsToUse, 1);
    }
}