using CoreCodedChatbot.ApiApplication.Repositories.Abstractions;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;
using CoreCodedChatbot.Database.DbExtensions;

namespace CoreCodedChatbot.ApiApplication.Repositories.Vip;

public class GetUsersVipCountRepository : BaseRepository<User>
{
    public GetUsersVipCountRepository(
        IChatbotContextFactory chatbotContextFactory
    ) : base(chatbotContextFactory)
    {
    }

    public int GetVips(string username)
    {
        var user = Context.GetOrCreateUser(username);

        if (user == null) return 0;

        var vipsReceived = user.DonationOrBitsVipRequests +
                           user.FollowVipRequest +
                           user.ModGivenVipRequests +
                           user.SubVipRequests +
                           user.TokenVipRequests +
                           user.ReceivedGiftVipRequests +
                           user.Tier2Vips +
                           (user.Tier3Vips * 2) +
                           user.ChannelPointVipRequests;

        var vipsUsed = user.UsedVipRequests + user.SentGiftVipRequests;

        return vipsReceived - vipsUsed;
    }
}