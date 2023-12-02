using System;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Repositories.Abstractions;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;
using CoreCodedChatbot.Database.DbExtensions;

namespace CoreCodedChatbot.ApiApplication.Repositories.Vip;

public class UpdateDonationVipsRepository : BaseRepository<User>
{
    public UpdateDonationVipsRepository(
        IChatbotContextFactory chatbotContextFactory
    ) : base(chatbotContextFactory)
    {
    }

    public async Task Update(string username, double bitsToVip, double donationAmountToVip)
    {
        var user = Context.GetOrCreateUser(username);

        var totalBitsGiven = user.TotalBitsDropped;
        var totalDonated = user.TotalDonated;

        var bitsVipPercentage = (double) totalBitsGiven / bitsToVip;
        var donationVipPercentage = (double) totalDonated / donationAmountToVip;

        user.DonationOrBitsVipRequests = (int) Math.Floor(bitsVipPercentage + donationVipPercentage);

        await Context.SaveChangesAsync();
    }
}