using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Bytes;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Vip;
using CoreCodedChatbot.ApiApplication.Interfaces.Queries.Bytes;
using CoreCodedChatbot.ApiApplication.Interfaces.Queries.Vip;
using CoreCodedChatbot.ApiApplication.Interfaces.Services;
using CoreCodedChatbot.ApiApplication.Models.Intermediates;
using CoreCodedChatbot.ApiContract.RequestModels.Vip.ChildModels;
using CoreCodedChatbot.Config;
using Microsoft.Extensions.Logging;

namespace CoreCodedChatbot.ApiApplication.Services
{
    public class VipService : IVipService
    {
        private readonly IGiftVipCommand _giftVipCommand;
        private readonly IRefundVipCommand _refundVipCommand;
        private readonly ICheckUserHasVipsQuery _checkUserHasVipsQuery;
        private readonly IUseVipCommand _useVipCommand;
        private readonly IUseSuperVipCommand _useSuperVipCommand;
        private readonly IModGiveVipCommand _modGiveVipCommand;
        private readonly IGetUsersGiftedVipsQuery _getUsersGiftedVipsQuery;
        private readonly IGetUserVipCountQuery _getUserVipCountQuery;
        private readonly IGiveSubscriptionVipsCommand _giveSubscriptionVipsCommand;
        private readonly IUpdateTotalBitsCommand _updateTotalBitsCommand;
        private readonly IGetUserByteCountQuery _getUserByteCountQuery;
        private readonly IConvertBytesCommand _convertBytesCommand;
        private readonly IConvertAllBytesCommand _convertAllBytesCommand;
        private readonly IGiveGiftSubBytesCommand _giveGiftSubBytesCommand;
        private readonly IConfigService _configService;
        private readonly ISignalRService _signalRService;
        private readonly IClientIdService _clientIdService;
        private readonly ILogger<IVipService> _logger;

        public VipService(
            IGiftVipCommand giftVipCommand,
            IRefundVipCommand refundVipCommand,
            ICheckUserHasVipsQuery checkUserHasVipsQuery,
            IUseVipCommand useVipCommand,
            IUseSuperVipCommand useSuperVipCommand,
            IModGiveVipCommand modGiveVipCommand,
            IGetUsersGiftedVipsQuery getUsersGiftedVipsQuery,
            IGetUserVipCountQuery getUserVipCountQuery,
            IGiveSubscriptionVipsCommand giveSubscriptionVipsCommand,
            IUpdateTotalBitsCommand updateTotalBitsCommand,
            IGetUserByteCountQuery getUserByteCountQuery,
            IConvertBytesCommand convertBytesCommand,
            IConvertAllBytesCommand convertAllBytesCommand,
            IGiveGiftSubBytesCommand giveGiftSubBytesCommand,
            IConfigService configService,
            ISignalRService signalRService,
            IClientIdService clientIdService,
            ILogger<IVipService> logger)
        {
            _giftVipCommand = giftVipCommand;
            _refundVipCommand = refundVipCommand;
            _checkUserHasVipsQuery = checkUserHasVipsQuery;
            _useVipCommand = useVipCommand;
            _useSuperVipCommand = useSuperVipCommand;
            _modGiveVipCommand = modGiveVipCommand;
            _getUsersGiftedVipsQuery = getUsersGiftedVipsQuery;
            _getUserVipCountQuery = getUserVipCountQuery;
            _giveSubscriptionVipsCommand = giveSubscriptionVipsCommand;
            _updateTotalBitsCommand = updateTotalBitsCommand;
            _getUserByteCountQuery = getUserByteCountQuery;
            _convertBytesCommand = convertBytesCommand;
            _convertAllBytesCommand = convertAllBytesCommand;
            _giveGiftSubBytesCommand = giveGiftSubBytesCommand;
            _configService = configService;
            _signalRService = signalRService;
            _clientIdService = clientIdService;
            _logger = logger;
        }

        public async Task UpdateClientVips(string username)
        {
            var vips = GetUserVipCount(username);

            var clientIds = _clientIdService.GetClientIds(username, "SongList");

            foreach (var clientId in clientIds)
            {
                await _signalRService.UpdateVips(clientId, vips);
            }
        }

        public async Task UpdateClientBytes(string username)
        {
            var bytes = _getUserByteCountQuery.Get(username);

            var clientIds = _clientIdService.GetClientIds(username, "SongList");
            
            await _signalRService.UpdateBytes(clientIds, bytes).ConfigureAwait(false);
        }

        public async Task<bool> GiftVip(string donorUsername, string receiverUsername, int numberOfVips)
        {
            var success = _giftVipCommand.GiftVip(donorUsername, receiverUsername, numberOfVips);

            if (success)
            {
                await UpdateClientVips(donorUsername).ConfigureAwait(false);
                await UpdateClientVips(receiverUsername).ConfigureAwait(false);
            }

            return success;
        }

        public async Task<bool> RefundVip(string username, bool deferSave = false)
        {
            try
            {
                _refundVipCommand.Refund(new VipRefund
                {
                    Username = username,
                    VipsToRefund = 1
                });

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
                _refundVipCommand.Refund(new VipRefund
                {
                    Username = username,
                    VipsToRefund = _configService.Get<int>("SuperVipCost")
                });

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
                var userHasVip = _checkUserHasVipsQuery.CheckUserHasVips(username, 1);

                return userHasVip;
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

                _useVipCommand.UseVip(username, 1);

                await UpdateClientVips(username).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error when attempting to deduct a user's Vip token. username: {username}");
                return false;
            }

            return true;
        }

        public bool HasSuperVip(string username)
        {
            try
            {
                var userHasVips = _checkUserHasVipsQuery.CheckUserHasVips(username, _configService.Get<int>("SuperVipCost"));

                return userHasVips;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error when checking if a user has enough for a Super Vip token. username: {username}");
                return false;
            }
        }

        public async Task<bool> UseSuperVip(string username)
        {
            try
            {
                if (!HasSuperVip(username)) return false;

                _useSuperVipCommand.UseSuperVip(username);

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
                _modGiveVipCommand.ModGiveVip(username, numberOfVips);

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
            var vips = _getUsersGiftedVipsQuery.GetUsersGiftedVips(username);

            return vips;
        }

        public int GetUserVipCount(string username)
        {
            var vips = _getUserVipCountQuery.Get(username);

            return vips;
        }

        public async Task GiveSubscriptionVips(List<UserSubDetail> usernames)
        {
            _giveSubscriptionVipsCommand.Give(usernames);

            foreach (var user in usernames)
            {
                await UpdateClientVips(user.Username).ConfigureAwait(false);
            }
        }

        public void UpdateTotalBits(string username, int totalBits)
        {
            _updateTotalBitsCommand.Update(username, totalBits);
        }

        public string GetUserByteCount(string username)
        {
            var bytes = _getUserByteCountQuery.Get(username);

            return bytes;
        }

        public async Task<int> ConvertBytes(string username, int requestedVips)
        {
            var bytesConverted = _convertBytesCommand.Convert(username, requestedVips);

            await UpdateClientVips(username).ConfigureAwait(false);
            await UpdateClientBytes(username).ConfigureAwait(false);

            return bytesConverted;
        }

        public async Task<int> ConvertAllBytes(string username)
        {
            var bytesConverted = _convertAllBytesCommand.Convert(username);

            await UpdateClientVips(username).ConfigureAwait(false);
            await UpdateClientBytes(username).ConfigureAwait(false);

            return bytesConverted;
        }

        public async void GiveGiftSubBytes(string username)
        {
            _giveGiftSubBytesCommand.Give(username);
            await UpdateClientBytes(username).ConfigureAwait(false);
        }
    }
}