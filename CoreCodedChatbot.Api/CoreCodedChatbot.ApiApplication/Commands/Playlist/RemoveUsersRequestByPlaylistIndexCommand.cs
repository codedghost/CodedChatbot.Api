using System.Linq;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Playlist;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Vip;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Playlist;
using CoreCodedChatbot.ApiApplication.Interfaces.Services;
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
        private readonly IVipService _vipService;

        public RemoveUsersRequestByPlaylistIndexCommand(
            IGetUsersRequestsRepository getUsersRequestsRepository,
            IArchiveRequestCommand archiveRequestCommand,
            IRefundVipCommand refundVipCommand,
            IConfigService configService,
            IVipService vipService
            )
        {
            _getUsersRequestsRepository = getUsersRequestsRepository;
            _archiveRequestCommand = archiveRequestCommand;
            _refundVipCommand = refundVipCommand;
            _configService = configService;
            _vipService = vipService;
        }

        public async Task<bool> Remove(string username, int playlistPosition)
        {
            var usersRequests = _getUsersRequestsRepository.GetUsersRequests(username);

            if (usersRequests == null || !usersRequests.Any()) return false;

            var request = usersRequests.SingleOrDefault(r => r.PlaylistPosition == playlistPosition);

            if (request == null) return false;

            await _archiveRequestCommand.ArchiveRequest(request.SongRequestId, true);

            return true;
        }
    }
}