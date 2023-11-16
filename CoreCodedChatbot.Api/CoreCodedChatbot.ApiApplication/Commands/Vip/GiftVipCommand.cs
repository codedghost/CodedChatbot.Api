using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Vip;
using CoreCodedChatbot.ApiApplication.Interfaces.Queries.Vip;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Vip;

namespace CoreCodedChatbot.ApiApplication.Commands.Vip;

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
        if (!_checkUserHasVipsQuery.CheckUserHasVips(donorUsername, vipsToGift)) return false;

        _giftVipRepository.GiftVip(donorUsername, receivingUsername, vipsToGift);
        return true;

    }
}