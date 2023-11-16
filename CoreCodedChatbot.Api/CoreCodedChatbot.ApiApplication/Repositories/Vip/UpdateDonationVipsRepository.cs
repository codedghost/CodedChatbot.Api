using System;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Vip;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.DbExtensions;

namespace CoreCodedChatbot.ApiApplication.Repositories.Vip;

public class UpdateDonationVipsRepository : IUpdateDonationVipsRepository
{
    private readonly IChatbotContextFactory _chatbotContextFactory;

    public UpdateDonationVipsRepository(
        IChatbotContextFactory chatbotContextFactory
    )
    {
        _chatbotContextFactory = chatbotContextFactory;
    }

    public void Update(string username, double bitsToVip, double donationAmountToVip)
    {
        using (var context = _chatbotContextFactory.Create())
        {
            var user = context.GetOrCreateUser(username);

            var totalBitsGiven = user.TotalBitsDropped;
            var totalDonated = user.TotalDonated;

            var bitsVipPercentage = (double)totalBitsGiven / bitsToVip;
            var donationVipPercentage = (double) totalDonated / donationAmountToVip;

            user.DonationOrBitsVipRequests = (int)Math.Floor(bitsVipPercentage + donationVipPercentage);

            context.SaveChanges();
        }
    }
}