using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Playlist;
using CoreCodedChatbot.ApiApplication.Interfaces.Queries.Playlist;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Playlist;
using CoreCodedChatbot.ApiApplication.Interfaces.Services;
using CoreCodedChatbot.ApiApplication.Models.Intermediates;
using CoreCodedChatbot.ApiContract.Enums.Playlist;

namespace CoreCodedChatbot.ApiApplication.Commands.Playlist
{
    public class ProcessSuperVipSongRequestCommand : IProcessSuperVipSongRequestCommand
    {
        private readonly IIsSuperVipInQueueQuery _isSuperVipInQueueQuery;
        private readonly IVipService _vipService;
        private readonly IAddRequestRepository _addRequestRepository;

        public ProcessSuperVipSongRequestCommand(
            IIsSuperVipInQueueQuery isSuperVipInQueueQuery,
            IVipService vipService,
            IAddRequestRepository addRequestRepository
            )
        {
            _isSuperVipInQueueQuery = isSuperVipInQueueQuery;
            _vipService = vipService;
            _addRequestRepository = addRequestRepository;
        }

        public async Task<AddSongResult> Process(string username, string requestText)
        {
            if (_isSuperVipInQueueQuery.IsSuperVipInQueue())
                return new AddSongResult
                {
                    AddRequestResult = AddRequestResult.OnlyOneSuper
                };

            if (!await _vipService.UseSuperVip(username, 0).ConfigureAwait(false))
                return new AddSongResult
                {
                    AddRequestResult = AddRequestResult.NotEnoughVips
                };

            return _addRequestRepository.AddRequest(requestText, username, false, true);
        }
    }
}