using System.Collections.Generic;

namespace CoreCodedChatbot.Api.Interfaces.Commands.Vip
{
    public interface IRefundVipCommand
    {
        void Refund(string username);
        void Refund(List<string> usernames);
    }
}