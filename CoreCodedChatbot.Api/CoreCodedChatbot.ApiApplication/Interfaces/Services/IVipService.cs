using System.Collections.Generic;
using CoreCodedChatbot.ApiContract.RequestModels.Vip.ChildModels;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Services
{
    public interface IVipService
    {
        bool GiftVip(string donorUsername, string receiverUsername, int numberOfVips);
        bool RefundVip(string username, bool deferSave = false);
        bool RefundSuperVip(string username, bool deferSave = false);
        bool HasVip(string username);
        bool UseVip(string username);
        bool HasSuperVip(string username);
        bool UseSuperVip(string username);
        bool ModGiveVip(string username, int numberOfVips);
        int GetUsersGiftedVips(string username);
        int GetUserVipCount(string username);
        void GiveSubscriptionVips(List<UserSubDetail> usernames);
        void UpdateTotalBits(string username, int totalBits);
        string GetUserByteCount(string username);
        int ConvertBytes(string username, int requestedVips);
        int ConvertAllBytes(string username);
        void GiveGiftSubBytes(string username);
    }
}