using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Playlist;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Playlist;
using CoreCodedChatbot.ApiContract.RequestModels.Playlist;

namespace CoreCodedChatbot.ApiApplication.Commands.Playlist
{
    public class EditRequestCommand : IEditRequestCommand
    {
        private readonly IEditRequestRepository _editRequestRepository;

        public EditRequestCommand(
            IEditRequestRepository editRequestRepository
            )
        {
            _editRequestRepository = editRequestRepository;
        }

        public void Edit(EditWebRequestRequestModel model, int songId)
        {
            _editRequestRepository.Edit(model.SongRequestId,
                string.IsNullOrWhiteSpace(model.Artist) && string.IsNullOrWhiteSpace(model.SelectedInstrument) ?
                    model.Title :
                $"{model.Artist} - {model.Title} ({model.SelectedInstrument})",
                model.Username, model.IsMod, songId);
        }
    }
}