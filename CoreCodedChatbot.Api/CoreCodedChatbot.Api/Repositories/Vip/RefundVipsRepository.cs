using System.Collections.Generic;
using CoreCodedChatbot.Api.Interfaces.Repositories.Vip;
using CoreCodedChatbot.Api.Models.Intermediates;
using CoreCodedChatbot.Database.Context.Interfaces;

namespace CoreCodedChatbot.Api.Repositories.Vip
{
    public class RefundVipsRepository : IRefundVipsRepository
    {
        private readonly IChatbotContextFactory _chatbotContextFactory;

        public RefundVipsRepository(
            IChatbotContextFactory chatbotContextFactory
            )
        {
            _chatbotContextFactory = chatbotContextFactory;
        }

        public void RefundVips(IEnumerable<VipRefund> refunds)
        {
            using (var context = _chatbotContextFactory.Create())
            {
                foreach (var refund in refunds)
                {
                    var user = context.Users.Find(refund.Username);

                    if (user == null) continue;

                    user.ModGivenVipRequests += refund.VipsToRefund;
                }

                context.SaveChanges();
            }
        }
    }
}