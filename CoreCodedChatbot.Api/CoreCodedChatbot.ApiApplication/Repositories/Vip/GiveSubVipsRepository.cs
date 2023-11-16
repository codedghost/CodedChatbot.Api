using System.Collections.Generic;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Vip;
using CoreCodedChatbot.ApiContract.Enums.VIP;
using CoreCodedChatbot.ApiContract.RequestModels.Vip.ChildModels;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.DbExtensions;
using Microsoft.Extensions.Logging;

namespace CoreCodedChatbot.ApiApplication.Repositories.Vip;

public class GiveSubVipsRepository : IGiveSubVipsRepository
{
    private readonly IChatbotContextFactory _chatbotContextFactory;
    private readonly ILogger<GiveSubVipsRepository> _logger;

    public GiveSubVipsRepository(
        IChatbotContextFactory chatbotContextFactory,
        ILogger<GiveSubVipsRepository> logger)
    {
        _chatbotContextFactory = chatbotContextFactory;
        _logger = logger;
    }

    public void Give(List<UserSubDetail> userSubDetails, int tier2ExtraVips, int tier3ExtraVips)
    {
        using (var context = _chatbotContextFactory.Create())
        {
            foreach (var userSubDetail in userSubDetails)
            {
                var user = context.GetOrCreateUser(userSubDetail.Username);

                _logger.LogInformation(
                    $"Updating {user.Username}'s VIPs - old Sub Months = {user.SubVipRequests}, new Sub Months = {userSubDetail.TotalSubMonths}. Sub Tier = {userSubDetail.SubscriptionTier}");

                user.SubVipRequests = userSubDetail.TotalSubMonths;
                switch (userSubDetail.SubscriptionTier)
                {
                    case SubscriptionTier.Tier2:
                        user.Tier2Vips += tier2ExtraVips;
                        break;
                    case SubscriptionTier.Tier3:
                        user.Tier3Vips += tier3ExtraVips;
                        break;
                    default:
                        break;
                }
            }

            context.SaveChanges();
        }
    }
}