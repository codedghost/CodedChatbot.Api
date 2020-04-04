using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Playlist;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Playlist;

namespace CoreCodedChatbot.ApiApplication.Commands.Playlist
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
            return songRequestId > 0 && _addSongToDriveRepository.AddSongToDrive(songRequestId);
        }
    }
}