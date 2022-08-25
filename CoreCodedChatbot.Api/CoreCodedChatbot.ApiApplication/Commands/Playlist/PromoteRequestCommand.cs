using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Playlist;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Playlist;
using CoreCodedChatbot.ApiApplication.Interfaces.Services;
using CoreCodedChatbot.ApiApplication.Models.Intermediates;
using CoreCodedChatbot.ApiContract.Enums.Playlist;

namespace CoreCodedChatbot.ApiApplication.Commands.Playlist
{
    public class PromoteRequestCommand : IPromoteRequestCommand
    {
        private readonly IPromoteUserRequestRepository _promoteUserRequestRepository;
        private readonly IVipService _vipService;
        private readonly IGetSongRequestByIdRepository _getSongRequestByIdRepository;

        public PromoteRequestCommand(
            IPromoteUserRequestRepository promoteUserRequestRepository,
            IVipService vipService,
            IGetSongRequestByIdRepository getSongRequestByIdRepository
            )
        {
            _promoteUserRequestRepository = promoteUserRequestRepository;
            _vipService = vipService;
            _getSongRequestByIdRepository = getSongRequestByIdRepository;
        }

        public async Task<PromoteRequestIntermediate> Promote(string username, bool useSuperVip, int songRequestId = 0)
        {
            var songRequest = _getSongRequestByIdRepository.GetRequest(songRequestId);

            if (useSuperVip)
            {
                if (songRequest.IsVip)
                {
                    if (!await _vipService.UseSuperVip(username, 1))
                    {
                        return new PromoteRequestIntermediate
                        {
                            PlaylistIndex = 0,
                            PromoteRequestResult = PromoteRequestResult.NoVipAvailable
                        };
                    }

                    return new PromoteRequestIntermediate
                    {
                        PlaylistIndex = 1,
                        PromoteRequestResult = PromoteRequestResult.Successful
                    };
                }

                if (songRequest.IsSuperVip)
                {
                    return new PromoteRequestIntermediate
                    {
                        PromoteRequestResult = PromoteRequestResult.AlreadyVip,
                        PlaylistIndex = 0
                    };
                }

                if (!await _vipService.UseSuperVip(username, 0))
                {
                    return new PromoteRequestIntermediate
                    {
                        PlaylistIndex = 0,
                        PromoteRequestResult = PromoteRequestResult.NoVipAvailable
                    };
                }

                return new PromoteRequestIntermediate
                {
                    PlaylistIndex = 1,
                    PromoteRequestResult = PromoteRequestResult.Successful
                };
            }

            if (songRequest?.IsVip ?? false)
            {
                return new PromoteRequestIntermediate
                {
                    PromoteRequestResult = PromoteRequestResult.AlreadyVip,
                    PlaylistIndex = 0
                };
            }

            if (!await _vipService.UseVip(username))
                return new PromoteRequestIntermediate
                {
                    PromoteRequestResult = PromoteRequestResult.NoVipAvailable
                };

            var newSongIndex = _promoteUserRequestRepository.PromoteUserRequest(username, songRequestId);

            return new PromoteRequestIntermediate
            {
                PromoteRequestResult =
                    newSongIndex > 0 ? PromoteRequestResult.Successful : PromoteRequestResult.UnSuccessful,
                PlaylistIndex = newSongIndex
            };
        }
    }
}