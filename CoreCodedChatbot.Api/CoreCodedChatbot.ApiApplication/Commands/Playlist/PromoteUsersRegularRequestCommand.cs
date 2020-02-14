using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Playlist;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Playlist;
using CoreCodedChatbot.ApiApplication.Interfaces.Services;
using CoreCodedChatbot.ApiApplication.Models.Intermediates;
using CoreCodedChatbot.ApiContract.Enums.Playlist;

namespace CoreCodedChatbot.ApiApplication.Commands.Playlist
{
    public class PromoteUsersRegularRequestCommand : IPromoteUsersRegularRequestCommand
    {
        private readonly IPromoteUserRequestRepository _promoteUserRequestRepository;
        private readonly IVipService _vipService;

        public PromoteUsersRegularRequestCommand(
            IPromoteUserRequestRepository promoteUserRequestRepository,
            IVipService vipService
            )
        {
            _promoteUserRequestRepository = promoteUserRequestRepository;
            _vipService = vipService;
        }

        public PromoteRequestIntermediate PromoteUsersRegularRequest(string username, int songRequestId = 0)
        {
            if (!_vipService.UseVip(username))
                return new PromoteRequestIntermediate
                {
                    PromoteRequestResult = PromoteRequestResult.NoVipAvailable
                };

            var newSongIndex = _promoteUserRequestRepository.PromoteUserRequest(username, songRequestId);

            return new PromoteRequestIntermediate
            {
                PromoteRequestResult =
                    newSongIndex == 0 ? PromoteRequestResult.UnSuccessful : PromoteRequestResult.Successful,
                PlaylistIndex = newSongIndex
            };
        }
    }
}