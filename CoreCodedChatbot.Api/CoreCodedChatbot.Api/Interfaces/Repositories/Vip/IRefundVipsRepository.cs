using System.Collections.Generic;

namespace CoreCodedChatbot.Api.Interfaces.Repositories.Vip
{
    public interface IRefundVipsRepository
    {
        void RefundVips(IEnumerable<string> usernames);
    }
}