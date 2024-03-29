﻿using System.Collections.Generic;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiContract.RequestModels.Vip.ChildModels;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Services;

public interface IVipService
{
    Task<bool> GiftVip(string donorUsername, string receiverUsername, int numberOfVips);
    Task<bool> RefundVip(string username, bool deferSave = false);
    Task<bool> RefundSuperVip(string username, bool deferSave = false);
    bool HasVip(string username);
    Task<bool> UseVip(string username);
    bool HasSuperVip(string username, int discount);
    Task<bool> UseSuperVip(string username, int discount);
    Task<bool> ModGiveVip(string username, int numberOfVips);
    int GetUsersGiftedVips(string username);
    int GetUserVipCount(string username);
    Task GiveSubscriptionVips(List<UserSubDetail> usernames);
    Task UpdateTotalBits(string username, int totalBits);
    string GetUserByteCount(string username);
    Task<int> ConvertBytes(string username, int requestedVips);
    Task<int> ConvertAllBytes(string username);
    Task GiveGiftSubBytes(string username);
    Task UpdateClientVips(string username);
    void GiveChannelPointsVip(string username);
}