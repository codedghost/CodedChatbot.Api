using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Playlist;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Playlist;
using CoreCodedChatbot.ApiApplication.Interfaces.Services;
using CoreCodedChatbot.ApiApplication.Models.Intermediates;
using CoreCodedChatbot.ApiContract.Enums.Playlist;

namespace CoreCodedChatbot.ApiApplication.Commands.Playlist
{
    public class ProcessVipSongRequestCommand : IProcessVipSongRequestCommand
    {
        private readonly IVipService _vipService;
        private readonly IAddRequestRepository _addRequestRepository;

        public ProcessVipSongRequestCommand(
            IVipService vipService,
            IAddRequestRepository addRequestRepository
            )
        {
            _vipService = vipService;
            _addRequestRepository = addRequestRepository;
        }

        public async Task<AddSongResult> Process(string username, string requestText)
        {
            if (!await _vipService.UseVip(username))
                return new AddSongResult
                {
                    AddRequestResult = AddRequestResult.NotEnoughVips
                };

            return _addRequestRepository.AddRequest(requestText, username, true, false);
        }
    }
}