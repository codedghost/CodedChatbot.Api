using System.Collections.Generic;
using CoreCodedChatbot.Api.Interfaces.Commands.Vip;
using CoreCodedChatbot.Api.Interfaces.Repositories.Vip;

namespace CoreCodedChatbot.Api.Commands.Vip
{
    public class RefundVipCommand : IRefundVipCommand
    {
        private readonly IRefundVipsRepository _refundVipsRepository;

        public RefundVipCommand(IRefundVipsRepository refundVipsRepository)
        {
            _refundVipsRepository = refundVipsRepository;
        }

        public void Refund(string username)
        {
            _refundVipsRepository.RefundVips(new List<string> {username});
        }

        public void Refund(List<string> usernames)
        {
            _refundVipsRepository.RefundVips(usernames);
        }
    }
}