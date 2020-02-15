using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Playlist;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Playlist;

namespace CoreCodedChatbot.ApiApplication.Commands.Playlist
{
    public class EditSuperVipCommand : IEditSuperVipCommand
    {
        private readonly IEditSuperVipRequestRepository _editSuperVipRequestRepository;

        public EditSuperVipCommand(
            IEditSuperVipRequestRepository editSuperVipRequestRepository
            )
        {
            _editSuperVipRequestRepository = editSuperVipRequestRepository;
        }

        public int Edit(string username, string newText)
        {
            var songId = _editSuperVipRequestRepository.Edit(username, newText);

            return songId;
        }
    }
}