using CoreCodedChatbot.Database.Context.Models;

namespace CoreCodedChatbot.Api.Extensions
{
    public static class UserDbExtensions
    {
        public static int UsableVips(this User user)
        {
            return (user.DonationOrBitsVipRequests + user.FollowVipRequest + user.ModGivenVipRequests +
                    user.SubVipRequests + user.TokenVipRequests + user.ReceivedGiftVipRequests) - user.UsedVipRequests -
                   user.SentGiftVipRequests;
        }
    }
}