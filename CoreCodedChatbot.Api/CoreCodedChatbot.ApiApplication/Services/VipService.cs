using System;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Vip;
using CoreCodedChatbot.ApiApplication.Interfaces.Queries.Vip;
using CoreCodedChatbot.ApiApplication.Models.Intermediates;
using CoreCodedChatbot.Config;
using Microsoft.Extensions.Logging;
using IVipService = CoreCodedChatbot.ApiApplication.Interfaces.Services.IVipService;

namespace CoreCodedChatbot.ApiApplication.Services
{
    public class VipService : Interfaces.Services.IVipService
    {
        private readonly IGiftVipCommand _giftVipCommand;
        private readonly IRefundVipCommand _refundVipCommand;
        private readonly ICheckUserHasVipsQuery _checkUserHasVipsQuery;
        private readonly IUseVipCommand _useVipCommand;
        private readonly IUseSuperVipCommand _useSuperVipCommand;
        private readonly IModGiveVipCommand _modGiveVipCommand;
        private readonly IGetUsersGiftedVipsQuery _getUsersGiftedVipsQuery;
        private readonly IGetUserVipCountQuery _getUserVipCountQuery;
        private readonly IConfigService _configService;
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
            IConfigService configService,
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
            _configService = configService;
            _logger = logger;
        }

        public bool GiftVip(string donorUsername, string receiverUsername)
        {
            var success = _giftVipCommand.GiftVip(donorUsername, receiverUsername, 1);

            return success;
        }

        public bool RefundVip(string username, bool deferSave = false)
        {
            try
            {
                _refundVipCommand.Refund(new VipRefund
                {
                    Username = username,
                    VipsToRefund = 1
                });

                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error when refunding Vip token. username: {username}, defersave: {deferSave}");
                return false;
            }
        }

        public bool RefundSuperVip(string username, bool deferSave = false)
        {
            try
            {
                _refundVipCommand.Refund(new VipRefund
                {
                    Username = username,
                    VipsToRefund = _configService.Get<int>("SuperVipCost")
                });

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

        public bool UseVip(string username)
        {
            try
            {
                if (!HasVip(username)) return false;

                _useVipCommand.UseVip(username, 1);
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

        public bool UseSuperVip(string username)
        {
            try
            {
                if (!HasSuperVip(username)) return false;

                _useSuperVipCommand.UseSuperVip(username);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error when attempting to deduct a user's Super Vip token. username: {username}");
                return false;
            }

            return true;
        }

        public bool ModGiveVip(string username, int numberOfVips)
        {
            try
            {
                _modGiveVipCommand.ModGiveVip(username, numberOfVips);
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
    }
}