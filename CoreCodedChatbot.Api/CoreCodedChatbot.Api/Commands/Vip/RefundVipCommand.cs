using System.Collections.Generic;
using CoreCodedChatbot.Api.Interfaces.Commands.Vip;
using CoreCodedChatbot.Api.Interfaces.Repositories.Vip;
using CoreCodedChatbot.Api.Models.Intermediates;

namespace CoreCodedChatbot.Api.Commands.Vip
{
    public class RefundVipCommand : IRefundVipCommand
    {
        private readonly IRefundVipsRepository _refundVipsRepository;

        public RefundVipCommand(IRefundVipsRepository refundVipsRepository)
        {
            _refundVipsRepository = refundVipsRepository;
        }

        public void Refund(VipRefund username)
        {
            _refundVipsRepository.RefundVips(new List<VipRefund> {username});
        }

        public void Refund(List<VipRefund> usernames)
        {
            _refundVipsRepository.RefundVips(usernames);
        }
    }
}