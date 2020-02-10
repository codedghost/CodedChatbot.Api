using System.Collections.Generic;
using CoreCodedChatbot.Api.Models.Intermediates;

namespace CoreCodedChatbot.Api.Interfaces.Commands.Vip
{
    public interface IRefundVipCommand
    {
        void Refund(VipRefund username);
        void Refund(List<VipRefund> usernames);
    }
}