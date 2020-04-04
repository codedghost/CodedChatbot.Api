using System.Linq;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Playlist;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Vip;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Playlist;
using CoreCodedChatbot.ApiApplication.Models.Intermediates;
using CoreCodedChatbot.Config;

namespace CoreCodedChatbot.ApiApplication.Commands.Playlist
{
    public class RemoveUsersRequestByPlaylistIndexCommand : IRemoveUsersRequestByPlaylistIndexCommand
    {
        private readonly IGetUsersRequestsRepository _getUsersRequestsRepository;
        private readonly IArchiveRequestCommand _archiveRequestCommand;
        private readonly IRefundVipCommand _refundVipCommand;
        private readonly IConfigService _configService;

        public RemoveUsersRequestByPlaylistIndexCommand(
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

        public bool Remove(string username, int playlistPosition)
        {
            var usersRequests = _getUsersRequestsRepository.GetUsersRequests(username);

            if (usersRequests == null || !usersRequests.Any()) return false;

            var request = usersRequests.SingleOrDefault(r => r.PlaylistPosition == playlistPosition);

            if (request == null) return false;

            if (request.IsVip || !request.IsSuperVip)
            {
                _refundVipCommand.Refund(new VipRefund
                {
                    Username = username,
                    VipsToRefund = request.IsSuperVip ? _configService.Get<int>("SuperVipCost") : 1
                });
            }

            _archiveRequestCommand.ArchiveRequest(request.SongRequestId);

            return true;
        }
    }
}