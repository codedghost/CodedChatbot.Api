using CoreCodedChatbot.Api.Interfaces.Commands.Vip;
using CoreCodedChatbot.Api.Interfaces.Repositories.Vip;

namespace CoreCodedChatbot.Api.Commands.Vip
{
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
}