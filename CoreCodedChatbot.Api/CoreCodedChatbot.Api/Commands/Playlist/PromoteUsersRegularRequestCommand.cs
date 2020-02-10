using CoreCodedChatbot.Api.Interfaces.Commands.Playlist;
using CoreCodedChatbot.Api.Interfaces.Repositories.Playlist;
using CoreCodedChatbot.Api.Interfaces.Services;
using CoreCodedChatbot.Api.Models.Intermediates;
using CoreCodedChatbot.ApiContract.Enums.Playlist;

namespace CoreCodedChatbot.Api.Commands.Playlist
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