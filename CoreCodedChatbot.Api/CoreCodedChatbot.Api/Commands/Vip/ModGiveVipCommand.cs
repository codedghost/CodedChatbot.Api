using CoreCodedChatbot.Api.Interfaces.Commands.Vip;
using CoreCodedChatbot.Api.Interfaces.Repositories.Vip;

namespace CoreCodedChatbot.Api.Commands.Vip
{
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
}