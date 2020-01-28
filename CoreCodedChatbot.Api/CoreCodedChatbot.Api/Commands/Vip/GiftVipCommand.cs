using CoreCodedChatbot.Api.Interfaces.Repositories.Vip;

namespace CoreCodedChatbot.Api.Commands.Vip
{
    public class GiftVipCommand
    {
        private readonly IGiftVipRepository _giftVipRepository;

        public GiftVipCommand(
            IGiftVipRepository giftVipRepository,

            )
        {
            _giftVipRepository = giftVipRepository;
        }

        public void GiftVip(string donorUsername, string receivingUsername)
        {

        }
    }
}