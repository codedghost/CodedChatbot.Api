using CoreCodedChatbot.Api.Interfaces.Commands.Playlist;
using CoreCodedChatbot.Api.Interfaces.Repositories.Playlist;

namespace CoreCodedChatbot.Api.Commands.Playlist
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