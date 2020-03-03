using System.Linq;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Playlist;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Vip;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Playlist;
using CoreCodedChatbot.ApiApplication.Models.Enums;
using CoreCodedChatbot.ApiApplication.Models.Intermediates;
using CoreCodedChatbot.Config;

namespace CoreCodedChatbot.ApiApplication.Commands.Playlist
{
    public class ArchiveUsersSingleRequest : IArchiveUsersSingleRequest
    {
        private readonly IGetUsersRequestsRepository _getUsersRequestsRepository;
        private readonly IArchiveRequestCommand _archiveRequestCommand;
        private readonly IRefundVipCommand _refundVipCommand;
        private readonly IConfigService _configService;

        public ArchiveUsersSingleRequest(
            IGetUsersRequestsRepository getUsersRequestsRepository,
            IArchiveRequestCommand archiveRequestCommand,
            IRefundVipCommand refundVipCommand,
            IConfigService configService
            )
        {
            _getUsersRequestsRepository = getUsersRequestsRepository;
            _archiveRequestCommand = archiveRequestCommand;
            _refundVipCommand = refundVipCommand;
            _configService = configService;
        }

        public bool ArchiveAndRefundVips(string username, SongRequestType songRequestType, int currentSongRequestId)
        {
            var usersRequests = _getUsersRequestsRepository.GetUsersRequests(username);

            int songRequestId = 0;
            int vipsToRefund = 0;
            switch (songRequestType)
            {
                case SongRequestType.Regular:
                    songRequestId = usersRequests.SingleOrDefault(r => !r.IsVip)?.SongRequestId ?? 0;
                    break;
                case SongRequestType.Vip:
                    songRequestId = usersRequests.SingleOrDefault(r => r.IsVip && !r.IsSuperVip)?.SongRequestId ?? 0;
                    vipsToRefund = 1;
                    break;
                case SongRequestType.SuperVip:
                    songRequestId = usersRequests.SingleOrDefault(r => r.IsSuperVip)?.SongRequestId ?? 0;
                    vipsToRefund = _configService.Get<int>("SuperVipCost");
                    break;
                default:
                    return false;
            }

            if (songRequestId == 0 || songRequestId == currentSongRequestId) return false;

            _archiveRequestCommand.ArchiveRequest(songRequestId);
            _refundVipCommand.Refund(new VipRefund
            {
                Username = username,
                VipsToRefund = vipsToRefund
            });

            return true;
        }
    }
}