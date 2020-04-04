using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Playlist;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Playlist;

namespace CoreCodedChatbot.ApiApplication.Commands.Playlist
{
    public class ArchiveRequestCommand : IArchiveRequestCommand
    {
        private readonly IArchiveRequestRepository _archiveRequestRepository;

        public ArchiveRequestCommand(
            IArchiveRequestRepository archiveRequestRepository
            )
        {
            _archiveRequestRepository = archiveRequestRepository;
        }

        public void ArchiveRequest(int requestId)
        {
            _archiveRequestRepository.ArchiveRequest(requestId);
        }
    }
}