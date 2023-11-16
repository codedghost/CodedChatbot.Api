using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Vip;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Vip;

namespace CoreCodedChatbot.ApiApplication.Commands.Vip;

public class ModGiveVipCommand : IModGiveVipCommand
{
    private readonly IModGiveVipRepository _modGiveVipRepository;

    public ModGiveVipCommand(
        IModGiveVipRepository modGiveVipRepository
    )
    {
        _modGiveVipRepository = modGiveVipRepository;
    }

    public void ModGiveVip(string username, int vipsToGive)
    {
        _modGiveVipRepository.ModGiveVip(username, vipsToGive);
    }
}