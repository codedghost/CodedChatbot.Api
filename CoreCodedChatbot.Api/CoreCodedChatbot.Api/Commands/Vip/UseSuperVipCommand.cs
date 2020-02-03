using CoreCodedChatbot.Api.Interfaces.Commands.Vip;
using CoreCodedChatbot.Api.Interfaces.Repositories.Vip;
using CoreCodedChatbot.Config;

namespace CoreCodedChatbot.Api.Commands.Vip
{
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

        public void UseSuperVip(string username)
        {
            var vipsToUse = _configService.Get<int>("SuperVipCost");

            _useSuperVipRepository.UseSuperVip(username, vipsToUse, 1);
        }
    }
}