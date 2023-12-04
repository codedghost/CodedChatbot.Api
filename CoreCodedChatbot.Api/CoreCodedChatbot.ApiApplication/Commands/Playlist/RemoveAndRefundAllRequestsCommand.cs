using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Playlist;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Vip;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Playlist;
using CoreCodedChatbot.ApiApplication.Interfaces.Services;
using CoreCodedChatbot.ApiApplication.Models.Intermediates;
using CoreCodedChatbot.ApiApplication.Repositories.Users;
using CoreCodedChatbot.Config;

namespace CoreCodedChatbot.ApiApplication.Commands.Playlist;

public class RemoveAndRefundAllRequestsCommand : IRemoveAndRefundAllRequestsCommand
{
    private readonly IGetCurrentRequestsRepository _getCurrentRequestsRepository;
    private readonly IClearRequestsRepository _clearRequestsRepository;
    private readonly IConfigService _configService;
    private readonly IVipService _vipService;

    public RemoveAndRefundAllRequestsCommand(
        IGetCurrentRequestsRepository getCurrentRequestsRepository,
        IClearRequestsRepository clearRequestsRepository,
        IConfigService configService,
        IVipService vipService
    )
    {
        _getCurrentRequestsRepository = getCurrentRequestsRepository;
        _clearRequestsRepository = clearRequestsRepository;
        _configService = configService;
        _vipService = vipService;
    }

    public async Task RemoveAndRefundAllRequests()
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

        using (var repo = new UsersRepository(_chatbotContextFactory, _configService, _logger))
        {
            await repo.RefundVips(refundVips);
        }

        foreach (var refund in refundVips)
        {
            await _vipService.UpdateClientVips(refund.Username).ConfigureAwait(false);
        }

        _clearRequestsRepository.ClearRequests(currentRequests.RegularRequests);
        _clearRequestsRepository.ClearRequests(currentRequests.VipRequests);
    }
}