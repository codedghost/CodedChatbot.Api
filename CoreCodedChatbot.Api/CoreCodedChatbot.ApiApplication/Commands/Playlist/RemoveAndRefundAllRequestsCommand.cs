using System.Collections.Generic;
using System.Linq;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Playlist;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Vip;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Playlist;
using CoreCodedChatbot.ApiApplication.Models.Intermediates;
using CoreCodedChatbot.Config;

namespace CoreCodedChatbot.ApiApplication.Commands.Playlist
{
    public class RemoveAndRefundAllRequestsCommand : IRemoveAndRefundAllRequestsCommand
    {
        private readonly IGetCurrentRequestsRepository _getCurrentRequestsRepository;
        private readonly IClearRequestsRepository _clearRequestsRepository;
        private readonly IRefundVipCommand _refundVipCommand;
        private readonly IConfigService _configService;

        public RemoveAndRefundAllRequestsCommand(
            IGetCurrentRequestsRepository getCurrentRequestsRepository,
            IClearRequestsRepository clearRequestsRepository,
            IRefundVipCommand refundVipCommand,
            IConfigService configService
            )
        {
            _getCurrentRequestsRepository = getCurrentRequestsRepository;
            _clearRequestsRepository = clearRequestsRepository;
            _refundVipCommand = refundVipCommand;
            _configService = configService;
        }

        public void RemoveAndRefundAllRequests()
        {
            var currentRequests = _getCurrentRequestsRepository.GetCurrentRequests();

            if (currentRequests == null)
                return;

            var refundVips = currentRequests?.VipRequests?.Where(sr => sr.IsSuperVip || sr.IsVip).Select(sr =>
                new VipRefund
                {
                    Username = sr.Username,
                    VipsToRefund = sr.IsSuperVip ? _configService.Get<int>("SuperVipCost") :
                        sr.IsVip ? 1 :
                            0
                }).ToList() ?? new List<VipRefund>();

            _refundVipCommand.Refund(refundVips);

            _clearRequestsRepository.ClearRequests(currentRequests.RegularRequests);
            _clearRequestsRepository.ClearRequests(currentRequests.VipRequests);
        }
    }
}