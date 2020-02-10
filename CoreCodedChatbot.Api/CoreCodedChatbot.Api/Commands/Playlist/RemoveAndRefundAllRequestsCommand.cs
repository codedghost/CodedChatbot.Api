using System.Linq;
using CoreCodedChatbot.Api.Interfaces.Commands.Playlist;
using CoreCodedChatbot.Api.Interfaces.Commands.Vip;
using CoreCodedChatbot.Api.Interfaces.Repositories.Playlist;
using CoreCodedChatbot.Api.Models.Intermediates;
using CoreCodedChatbot.Config;

namespace CoreCodedChatbot.Api.Commands.Playlist
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

            var refundVips = currentRequests.SongRequests.Where(sr => sr.IsSuperVip || sr.IsVip).Select(sr =>
                new VipRefund
                {
                    Username = sr.Username,
                    VipsToRefund = sr.IsVip ? 1 : sr.IsSuperVip ? _configService.Get<int>("SuperVipCost") : 0
                }).ToList();

            _refundVipCommand.Refund(refundVips);

            _clearRequestsRepository.ClearRequests(currentRequests.SongRequests);
        }
    }
}