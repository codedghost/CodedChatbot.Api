using System.Collections.Generic;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Services
{
    public interface IVipService
    {
        bool GiftVip(string donorUsername, string receiverUsername);
        bool RefundVip(string username, bool deferSave = false);
        bool RefundSuperVip(string username, bool deferSave = false);
        bool HasVip(string username);
        bool UseVip(string username);
        bool HasSuperVip(string username);
        bool UseSuperVip(string username);
        bool ModGiveVip(string username, int numberOfVips);
        int GetUsersGiftedVips(string username);
        int GetUserVipCount(string username);
        void GiveSubscriptionVips(List<string> usernames);
        void UpdateTotalBits(string username, int totalBits);
        string GetUserByteCount(string username);
        int ConvertBytes(string username, int requestedVips);
        int ConvertAllBytes(string username);
    }
}