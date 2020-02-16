using System.Collections.Generic;
using CoreCodedChatbot.ApiApplication.Models.Intermediates;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Commands.Vip
{
    public interface IRefundVipCommand
    {
        void Refund(VipRefund vipRefund);
        void Refund(List<VipRefund> vipRefunds);
    }
}