using System.Collections.Generic;
using CoreCodedChatbot.ApiApplication.Models.Intermediates;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Commands.Vip
{
    public interface IRefundVipCommand
    {
        void Refund(VipRefund username);
        void Refund(List<VipRefund> usernames);
    }
}