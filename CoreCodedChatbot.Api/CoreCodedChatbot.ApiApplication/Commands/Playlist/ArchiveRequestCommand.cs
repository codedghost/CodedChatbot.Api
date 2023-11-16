using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Playlist;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Vip;
using CoreCodedChatbot.ApiApplication.Interfaces.Queries.Playlist;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Playlist;
using CoreCodedChatbot.ApiApplication.Interfaces.Services;
using CoreCodedChatbot.ApiApplication.Models.Intermediates;
using CoreCodedChatbot.Config;

namespace CoreCodedChatbot.ApiApplication.Commands.Playlist;

public class ArchiveRequestCommand : IArchiveRequestCommand
{
    private readonly IArchiveRequestRepository _archiveRequestRepository;
    private readonly IGetSongRequestByIdQuery _getSongRequestByIdQuery;
    private readonly IRefundVipCommand _refundVipCommand;
    private readonly IVipService _vipService;
    private readonly IConfigService _configService;

    public ArchiveRequestCommand(
        IArchiveRequestRepository archiveRequestRepository,
        IGetSongRequestByIdQuery getSongRequestByIdQuery,
        IRefundVipCommand refundVipCommand,
        IVipService vipService,
        IConfigService configService
    )
    {
        _archiveRequestRepository = archiveRequestRepository;
        _getSongRequestByIdQuery = getSongRequestByIdQuery;
        _refundVipCommand = refundVipCommand;
        _vipService = vipService;
        _configService = configService;
    }

    public async Task ArchiveRequest(int requestId, bool refundVip)
    {
        var username = _archiveRequestRepository.ArchiveRequest(requestId);

        if (refundVip)
        {
            var request = _getSongRequestByIdQuery.GetSongRequestById(requestId);
            if (request.isVip || request.isSuperVip)
            {
                _refundVipCommand.Refund(new VipRefund
                {
                    Username = request.songRequester,
                    VipsToRefund = request.isVip ? 1 : _configService.Get<int>("SuperVipCost")
                });
            }
        }

        await _vipService.UpdateClientVips(username).ConfigureAwait(false);
    }
}