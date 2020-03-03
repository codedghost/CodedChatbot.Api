using System.Linq;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Playlist;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Playlist;

namespace CoreCodedChatbot.ApiApplication.Commands.Playlist
{
    public class RemoveUsersRequestByPlaylistIndexCommand : IRemoveUsersRequestByPlaylistIndexCommand
    {
        private readonly IGetUsersRequestsRepository _getUsersRequestsRepository;
        private readonly IArchiveRequestCommand _archiveRequestCommand;

        public RemoveUsersRequestByPlaylistIndexCommand(
            IGetUsersRequestsRepository getUsersRequestsRepository,
            IArchiveRequestCommand archiveRequestCommand
            )
        {
            _getUsersRequestsRepository = getUsersRequestsRepository;
            _archiveRequestCommand = archiveRequestCommand;
        }

        public bool Remove(string username, int playlistPosition)
        {
            var usersRequests = _getUsersRequestsRepository.GetUsersRequests(username);

            if (usersRequests == null || !usersRequests.Any()) return false;

            var request = usersRequests.SingleOrDefault(r => r.PlaylistPosition == playlistPosition);

            if (request == null) return false;

            _archiveRequestCommand.ArchiveRequest(request.SongRequestId);

            return true;
        }
    }
}