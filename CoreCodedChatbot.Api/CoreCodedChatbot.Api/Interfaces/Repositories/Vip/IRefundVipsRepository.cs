using System.Collections.Generic;
using CoreCodedChatbot.Api.Intermediates;

namespace CoreCodedChatbot.Api.Interfaces.Repositories.Vip
{
    public interface IRefundVipsRepository
    {
        void RefundVips(IEnumerable<VipRefund> usernames);
    }
}