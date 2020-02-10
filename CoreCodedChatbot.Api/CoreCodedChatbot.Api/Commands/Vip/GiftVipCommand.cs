using CoreCodedChatbot.Api.Interfaces.Commands.Quote;
using CoreCodedChatbot.Api.Interfaces.Queries.Vip;
using CoreCodedChatbot.Api.Interfaces.Repositories.Vip;

namespace CoreCodedChatbot.Api.Commands.Vip
{
    public class GiftVipCommand : IGiftVipCommand
    {
        private readonly IGiftVipRepository _giftVipRepository;
        private readonly ICheckUserHasVipsQuery _checkUserHasVipsQuery;

        public GiftVipCommand(
            IGiftVipRepository giftVipRepository,
            ICheckUserHasVipsQuery checkUserHasVipsQuery
            )
        {
            _giftVipRepository = giftVipRepository;
            _checkUserHasVipsQuery = checkUserHasVipsQuery;
        }

        public bool GiftVip(string donorUsername, string receivingUsername, int vipsToGift)
        {
            if (_checkUserHasVipsQuery.CheckUserHasVips(donorUsername, vipsToGift))
            {
                _giftVipRepository.GiftVip(donorUsername, receivingUsername, vipsToGift);
            }

            return false;
        }
    }
}