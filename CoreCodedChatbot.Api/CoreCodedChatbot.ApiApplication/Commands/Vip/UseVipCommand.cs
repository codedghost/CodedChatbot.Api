using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Vip;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Vip;

namespace CoreCodedChatbot.ApiApplication.Commands.Vip;

public class UseVipCommand : IUseVipCommand
{
    private readonly IUseVipRepository _useVipRepository;

    public UseVipCommand(
        IUseVipRepository useVipRepository
    )
    {
        _useVipRepository = useVipRepository;
    }

    public void UseVip(string username, int vips)
    {
        _useVipRepository.UseVip(username, vips);
    }
}