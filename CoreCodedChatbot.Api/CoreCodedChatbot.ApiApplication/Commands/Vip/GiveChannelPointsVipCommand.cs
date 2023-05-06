using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Vip;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Vip;

namespace CoreCodedChatbot.ApiApplication.Commands.Vip
{
    public class GiveChannelPointsVipCommand : IGiveChannelPointsVipCommand
    {
        private readonly IGiveChannelPointsVipRepository _giveChannelPointsVipRepository;

        public GiveChannelPointsVipCommand(IGiveChannelPointsVipRepository giveChannelPointsVipRepository)
        {
            _giveChannelPointsVipRepository = giveChannelPointsVipRepository;
        }

        public void GiveChannelPointsVip(string username)
        {
            _giveChannelPointsVipRepository.GiveChannelPointsVip(username);
        }
    }
}