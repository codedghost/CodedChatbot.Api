using System.Collections.Generic;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Vip;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Vip;
using CoreCodedChatbot.ApiApplication.Models.Intermediates;

namespace CoreCodedChatbot.ApiApplication.Commands.Vip;

public class RefundVipCommand : IRefundVipCommand
{
    private readonly IRefundVipsRepository _refundVipsRepository;

    public RefundVipCommand(IRefundVipsRepository refundVipsRepository)
    {
        _refundVipsRepository = refundVipsRepository;
    }

    public void Refund(VipRefund vipRefund)
    {
        _refundVipsRepository.RefundVips(new List<VipRefund> {vipRefund});
    }

    public void Refund(List<VipRefund> vipRefunds)
    {
        _refundVipsRepository.RefundVips(vipRefunds);
    }
}