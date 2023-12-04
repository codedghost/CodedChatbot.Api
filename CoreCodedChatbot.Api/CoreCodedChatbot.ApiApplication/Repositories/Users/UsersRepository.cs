using CoreCodedChatbot.ApiApplication.Models.Intermediates;
using CoreCodedChatbot.ApiApplication.Repositories.Abstractions;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;
using CoreCodedChatbot.Database.DbExtensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreCodedChatbot.Config;
using CoreCodedChatbot.ApiContract.Enums.VIP;
using CoreCodedChatbot.ApiContract.RequestModels.Vip.ChildModels;
using Microsoft.Extensions.Logging;

namespace CoreCodedChatbot.ApiApplication.Repositories.Users;

public class UsersRepository : BaseRepository<User>
{
    private readonly IConfigService _configService;
    private readonly ILogger _logger;

    public UsersRepository(
        IChatbotContextFactory chatbotContextFactory, 
        IConfigService configService,
        ILogger logger) 
        : base(chatbotContextFactory)
    {
        _configService = configService;
        _logger = logger;
    }

    #region Bytes

    public async Task GiveBytes(List<GiveBytesToUserModel> users)
    {
        foreach (var winner in users)
        {
            // Find or add user
            var user = Context.GetOrCreateUser(winner.Username);

            user.TokenBytes += (int)Math.Round(winner.Bytes * _configService.Get<int>("BytesToVip"));
        }

        await Context.SaveChangesAsync();
    }

    public async Task<int> ConvertBytes(string username, int tokensToConvert, int byteConversion)
    {
        var user = Context.GetOrCreateUser(username);

        if (tokensToConvert < 0 || (user.TokenBytes < byteConversion * tokensToConvert)) return 0;

        var bytesToRemove = byteConversion * tokensToConvert;

        user.TokenBytes -= bytesToRemove;
        user.TokenVipRequests += tokensToConvert;

        await Context.SaveChangesAsync();

        return tokensToConvert;
    }

    public float GetUserByteCount(string username, int byteConversion)
    {
        var user = Context.GetOrCreateUser(username);

        return user.TokenBytes / (float)byteConversion;
    }

    public async Task GiveGiftSubBytes(string username, int conversionAmount)
    {
        var user = Context.GetOrCreateUser(username);
        var totalBytes = conversionAmount / 2;

        user.TokenBytes += totalBytes;

        await Context.SaveChangesAsync();
    }

    #endregion

    #region ClientIds

    public async Task<List<string>> GetClientIds(string username, string hubType)
    {
        var user = await GetByIdOrNullAsync(username);

        if (user == null) return new List<string>();

        var clientIds = user.GetClientIdsDictionary();

        return clientIds.ContainsKey(hubType) ? clientIds[hubType] : new List<string>();
    }

    public async Task RemoveClientId(string hubType, string clientId)
    {
        Context.Users.RemoveClientId(hubType, clientId);

        await Context.SaveChangesAsync();
    }

    public async Task StoreClientId(string hubType, string username, string clientId)
    {
        var user = Context.GetOrCreateUser(username);

        var clientIds = user.GetClientIdsDictionary();

        if (!clientIds.ContainsKey(hubType))
        {
            clientIds.Add(hubType, new List<string>());
        }

        clientIds[hubType].Add(clientId);

        user.UpdateClientIdsDictionary(clientIds);

        await Context.SaveChangesAsync();
    }

    #endregion

    #region Moderation

    public async Task TransferUser(string moderatorUsername, string oldUsername, string newUsername)
    {
        // All users should exist int the db at this point
        Context.TransferUser(moderatorUsername, oldUsername, newUsername);

        await Context.SaveChangesAsync();
    }

    #endregion

    #region StreamLabs

    public async Task UpdateUsersDonations(List<StreamLabsDonation> groupedDonations)
    {
        foreach (var donation in groupedDonations)
        {
            var user = Context.GetOrCreateUser(donation.Name);
            user.TotalDonated += donation.Amount;
            await Context.SaveChangesAsync();
        }
    }

    #endregion

    #region VIPs

    public int GetUsersGiftedVips(string username)
    {
        var user = Context.GetOrCreateUser(username);

        return user.SentGiftVipRequests;
    }

    public int GetUsersVipCount(string username)
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

    public async Task GiveChannelPointsVip(string username)
    {
        var user = Context.GetOrCreateUser(username);

        user.ChannelPointVipRequests++;

        await Context.SaveChangesAsync();
    }

    public async Task GiveSubVips(List<UserSubDetail> userSubDetails, int tier2ExtraVips, int tier3ExtraVips)
    {
        foreach (var userSubDetail in userSubDetails)
        {
            var user = Context.GetOrCreateUser(userSubDetail.Username);

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
            }
        }

        await Context.SaveChangesAsync();
    }

    public async Task GiftVip(string donorUsername, string receivingUsername, int vipsToGift)
    {
        var donorUser = await GetByIdAsync(donorUsername);
        var receivingUser = Context.GetOrCreateUser(receivingUsername);

        donorUser.SentGiftVipRequests += vipsToGift;
        receivingUser.ReceivedGiftVipRequests += vipsToGift;

        await Context.SaveChangesAsync();
    }

    public async Task ModGiveVip(string username, int vipsToGive)
    {
        var user = Context.GetOrCreateUser(username);

        user.ModGivenVipRequests += vipsToGive;

        await Context.SaveChangesAsync();
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

    public async Task UpdateDonationVips(string username, double bitsToVip, double donationAmountToVip)
    {
        var user = Context.GetOrCreateUser(username);

        var totalBitsGiven = user.TotalBitsDropped;
        var totalDonated = user.TotalDonated;

        var bitsVipPercentage = (double)totalBitsGiven / bitsToVip;
        var donationVipPercentage = (double)totalDonated / donationAmountToVip;

        user.DonationOrBitsVipRequests = (int)Math.Floor(bitsVipPercentage + donationVipPercentage);

        await Context.SaveChangesAsync();
    }

    public async Task UpdateTotalBits(string username, int totalBits)
    {
        var user = Context.GetOrCreateUser(username);

        user.TotalBitsDropped = totalBits;

        await Context.SaveChangesAsync();
    }

    public async Task UseSuperVip(string username, int vipsToUse, int superVipsToRegister)
    {
        var user = await GetByIdOrNullAsync(username);

        if (user == null) throw new UnauthorizedAccessException("User does not exist");

        user.UsedSuperVipRequests += superVipsToRegister;
        user.UsedVipRequests += vipsToUse;

        await Context.SaveChangesAsync();
    }

    public async Task UseVip(string username, int vips)
    {
        var user = await GetByIdOrNullAsync(username);

        if (user == null) throw new UnauthorizedAccessException("User does not exist");

        user.UsedVipRequests += vips;

        await Context.SaveChangesAsync();
    }

    #endregion

    #region WatchTime

    public async Task<TimeSpan> GetWatchTime(string username)
    {
        var user = await GetByIdOrNullAsync(username);

        return user == null ? TimeSpan.Zero : TimeSpan.FromMinutes(user.WatchTime);
    }

    public async Task UpdateWatchTime(IEnumerable<string> chatters)
    {
        foreach (var username in chatters)
        {
            var user = Context.GetOrCreateUser(username);

            user.WatchTime++;
            user.TimeLastInChat = DateTime.UtcNow;
        }

        await Context.SaveChangesAsync();
    }

    #endregion
}