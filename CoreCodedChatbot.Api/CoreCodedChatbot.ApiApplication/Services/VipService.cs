using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Interfaces.Services;
using CoreCodedChatbot.ApiApplication.Models.Intermediates;
using CoreCodedChatbot.ApiApplication.Repositories.Users;
using CoreCodedChatbot.ApiContract.RequestModels.Vip.ChildModels;
using CoreCodedChatbot.Config;
using CoreCodedChatbot.Database.Context.Interfaces;
using Microsoft.Extensions.Logging;

namespace CoreCodedChatbot.ApiApplication.Services;

public class VipService : IBaseService, IVipService
{
    private readonly IChatbotContextFactory _chatbotContextFactory;
    private readonly IConfigService _configService;
    private readonly ISignalRService _signalRService;
    private readonly IClientIdService _clientIdService;
    private readonly ILogger<IVipService> _logger;

    public VipService(
        IChatbotContextFactory chatbotContextFactory,
        IConfigService configService,
        ISignalRService signalRService,
        IClientIdService clientIdService,
        ILogger<IVipService> logger)
    {
        _chatbotContextFactory = chatbotContextFactory;
        _configService = configService;
        _signalRService = signalRService;
        _clientIdService = clientIdService;
        _logger = logger;
    }

    public async Task UpdateClientVips(string username)
    {
        var vips = GetUserVipCount(username);

        var clientIds = await _clientIdService.GetClientIds(username, "SongList");

        foreach (var clientId in clientIds)
        {
            await _signalRService.UpdateVips(clientId, vips);
        }
    }

    public async void GiveChannelPointsVip(string username)
    {
        using (var repo = new UsersRepository(_chatbotContextFactory, _configService, _logger))
        {
            await repo.GiveChannelPointsVip(username);
        }
        
        await UpdateClientVips(username).ConfigureAwait(false);
    }

    public async Task UpdateClientBytes(string username)
    {
        var bytes = Get(username);

        var clientIds = await _clientIdService.GetClientIds(username, "SongList");
            
        await _signalRService.UpdateBytes(clientIds, bytes).ConfigureAwait(false);
    }

    public async Task<bool> GiftVip(string donorUsername, string receiverUsername, int numberOfVips)
    {
        using (var repo = new UsersRepository(_chatbotContextFactory, _configService, _logger))
        {
            var hasVips = repo.GetUsersVipCount(donorUsername) >= numberOfVips;
            if (!hasVips) return false;

            await repo.GiftVip(donorUsername, receiverUsername, numberOfVips);
        }

        await UpdateClientVips(donorUsername).ConfigureAwait(false);
        await UpdateClientVips(receiverUsername).ConfigureAwait(false);

        return true;
    }

    public async Task<bool> RefundVip(string username, bool deferSave = false)
    {
        try
        {
            using (var repo = new UsersRepository(_chatbotContextFactory, _configService, _logger))
            {
                await repo.RefundVips(new List<VipRefund>
                {
                    new VipRefund
                    {
                        Username = username,
                        VipsToRefund = 1
                    }
                });
            }

            await UpdateClientVips(username).ConfigureAwait(false);

            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error when refunding Vip token. username: {username}, defersave: {deferSave}");
            return false;
        }
    }

    public async Task<bool> RefundSuperVip(string username, bool deferSave = false)
    {
        try
        {
            using (var repo = new UsersRepository(_chatbotContextFactory, _configService, _logger))
            {
                await repo.RefundVips(new List<VipRefund>
                {
                    new VipRefund
                    {
                        Username = username,
                        VipsToRefund = _configService.Get<int>("SuperVipCost")
                    }
                });
            }

            await UpdateClientVips(username).ConfigureAwait(false);

            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error when refunding Super Vip. username: {username}, deferSave: {deferSave}");
            return false;
        }
    }

    public bool HasVip(string username)
    {
        try
        {
            using (var repo = new UsersRepository(_chatbotContextFactory, _configService, _logger))
            {
                return repo.GetUsersVipCount(username) >= 1;
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error when checking if User has a Vip token. username: {username}");
            return false;
        }
    }

    public async Task<bool> UseVip(string username)
    {
        try
        {
            if (!HasVip(username)) return false;

            using (var repo = new UsersRepository(_chatbotContextFactory, _configService, _logger))
            {
                await repo.UseVip(username, 1);
            }

            await UpdateClientVips(username).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error when attempting to deduct a user's Vip token. username: {username}");
            return false;
        }

        return true;
    }

    public bool HasSuperVip(string username, int discount = 0)
    {
        try
        {
            using (var repo = new UsersRepository(_chatbotContextFactory, _configService, _logger))
            {
                return repo.GetUsersVipCount(username) >= _configService.Get<int>("SuperVipCost") - discount;
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error when checking if a user has enough for a Super Vip token. username: {username}");
            return false;
        }
    }

    public async Task<bool> UseSuperVip(string username, int discount = 0)
    {
        try
        {
            if (!HasSuperVip(username, discount)) return false;

            var vipsToUse = _configService.Get<int>("SuperVipCost") - discount;

            using (var repo = new UsersRepository(_chatbotContextFactory, _configService, _logger))
            {
                await repo.UseSuperVip(username, vipsToUse, 1);
            }

            await UpdateClientVips(username).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error when attempting to deduct a user's Super Vip token. username: {username}");
            return false;
        }

        return true;
    }

    public async Task<bool> ModGiveVip(string username, int numberOfVips)
    {
        try
        {
            using (var repo = new UsersRepository(_chatbotContextFactory, _configService, _logger))
            {
                await repo.ModGiveVip(username, numberOfVips);
            }

            await UpdateClientVips(username).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error when a mod has attempted to give 1 or more Vips to a user. username: {username}, numberOfVips: {numberOfVips}");
            return false;
        }

        return true;
    }

    public int GetUsersGiftedVips(string username)
    {
        using (var repo = new UsersRepository(_chatbotContextFactory, _configService, _logger))
        {
            return repo.GetUsersGiftedVips(username);
        }
    }

    public int GetUserVipCount(string username)
    {
        using (var repo = new UsersRepository(_chatbotContextFactory, _configService, _logger))
        {
            return repo.GetUsersVipCount(username);
        }
    }

    public async Task GiveSubscriptionVips(List<UserSubDetail> usernames)
    {
        var tier2ExtraVips = _configService.Get<int>("Tier2ExtraVip");
        var tier3ExtraVips = _configService.Get<int>("Tier3ExtraVip");

        using (var repo = new UsersRepository(_chatbotContextFactory, _configService, _logger))
        {
            await repo.GiveSubVips(usernames, tier2ExtraVips, tier3ExtraVips);
        }

        foreach (var user in usernames)
        {
            await UpdateClientVips(user.Username).ConfigureAwait(false);
        }
    }

    public async Task UpdateTotalBits(string username, int totalBits)
    {
        using (var repo = new UsersRepository(_chatbotContextFactory, _configService, _logger))
        {
            await repo.UpdateTotalBits(username, totalBits);
            var bitsToVip = _configService.Get<double>("BitsToVip");
            var donationAmountToVip = _configService.Get<double>("DonationAmountToVip");

            await repo.UpdateDonationVips(username, bitsToVip, donationAmountToVip);
        }
    }

    public string GetUserByteCount(string username)
    {
        var bytes = Get(username);

        return bytes;
    }

    public async Task<int> ConvertBytes(string username, int requestedVips)
    {
        var conversionAmount = _configService.Get<int>("BytesToVip");

        int bytesConverted;
        using (var repo = new UsersRepository(_chatbotContextFactory, _configService, _logger))
        {
            bytesConverted = await repo.ConvertBytes(username, requestedVips, conversionAmount);
        }

        await UpdateClientVips(username).ConfigureAwait(false);
        await UpdateClientBytes(username).ConfigureAwait(false);

        return bytesConverted;
    }

    public async Task<int> ConvertAllBytes(string username)
    {
        var conversionAmount = _configService.Get<int>("BytesToVip");

        float usersBytes;
        using (var repo = new UsersRepository(_chatbotContextFactory, _configService, _logger))
        {
            usersBytes = repo.GetUserByteCount(username, conversionAmount);
        }

        var closestWholeBytes = (int)Math.Floor(usersBytes);

        int bytesConverted;
        using (var repo = new UsersRepository(_chatbotContextFactory, _configService, _logger))
        {
            bytesConverted = await repo.ConvertBytes(username, closestWholeBytes, conversionAmount);
        }

        await UpdateClientVips(username).ConfigureAwait(false);
        await UpdateClientBytes(username).ConfigureAwait(false);

        return bytesConverted;
    }

    public async Task GiveGiftSubBytes(string username)
    {
        var conversionAmount = _configService.Get<int>("BytesToVip");

        using (var repo = new UsersRepository(_chatbotContextFactory, _configService, _logger))
        {
            await repo.GiveGiftSubBytes(username, conversionAmount);
        }
        await UpdateClientBytes(username).ConfigureAwait(false);
    }

    private string Get(string username)
    {
        var conversionAmount = _configService.Get<int>("BytesToVip");

        using (var repo = new UsersRepository(_chatbotContextFactory, _configService, _logger))
        {
            var bytes = repo.GetUserByteCount(username, conversionAmount);

            return bytes.ToString("n3");
        }
    }
}