using CoreCodedChatbot.Api.Interfaces.Commands.Playlist;
using CoreCodedChatbot.Api.Interfaces.Repositories.Playlist;

namespace CoreCodedChatbot.Api.Commands.Playlist
{
    public class AddSongToDriveCommand : IAddSongToDriveCommand
    {
        private readonly IAddSongToDriveRepository _addSongToDriveRepository;

        public AddSongToDriveCommand(
            IAddSongToDriveRepository addSongToDriveRepository
            )
        {
            _addSongToDriveRepository = addSongToDriveRepository;
        }

        public bool AddSongToDrive(int songRequestId)
        {
            return _addSongToDriveRepository.AddSongToDrive(songRequestId);
        }
    }
}