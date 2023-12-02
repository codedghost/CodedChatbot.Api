using System.Collections.Generic;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Models.Intermediates;
using CoreCodedChatbot.ApiApplication.Repositories.Abstractions;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;

namespace CoreCodedChatbot.ApiApplication.Repositories.Vip;

public class RefundVipsRepository : BaseRepository<User>
{
    public RefundVipsRepository(
        IChatbotContextFactory chatbotContextFactory
    ) : base(chatbotContextFactory)
    {
    }

    public async Task RefundVips(IEnumerable<VipRefund> refunds)
    {
        foreach (var refund in refunds)
        {
            var user = await GetByIdOrNullAsync(refund.Username);

            if (user == null) continue;

            user.ModGivenVipRequests += refund.VipsToRefund;
        }

        await Context.SaveChangesAsync();
    }
}