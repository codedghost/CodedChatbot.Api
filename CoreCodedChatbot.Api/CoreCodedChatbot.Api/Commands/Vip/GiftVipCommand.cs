using CoreCodedChatbot.Api.Interfaces.Commands.Quote;
using CoreCodedChatbot.Api.Interfaces.Repositories.Vip;

namespace CoreCodedChatbot.Api.Commands.Vip
{
    public class GiftVipCommand : IGiftVipCommand
    {
        private readonly IGiftVipRepository _giftVipRepository;
        private readonly IUserHasVipsRepository _userHasVipsRepository;

        public GiftVipCommand(
            IGiftVipRepository giftVipRepository,
            IUserHasVipsRepository userHasVipsRepository
            )
        {
            _giftVipRepository = giftVipRepository;
            _userHasVipsRepository = userHasVipsRepository;
        }

        public bool GiftVip(string donorUsername, string receivingUsername, int vipsToGift)
        {
            if (_userHasVipsRepository.HasVips(donorUsername, vipsToGift))
            {
                _giftVipRepository.GiftVip(donorUsername, receivingUsername, vipsToGift);
            }

            return false;
        }
    }
}