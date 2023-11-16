using System.Collections.Generic;
using CoreCodedChatbot.ApiApplication.Models.Intermediates;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Vip;

public interface IRefundVipsRepository
{
    void RefundVips(IEnumerable<VipRefund> usernames);
}