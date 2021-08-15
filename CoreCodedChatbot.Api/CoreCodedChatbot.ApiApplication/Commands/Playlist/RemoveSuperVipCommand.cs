using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Playlist;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Playlist;
using CoreCodedChatbot.ApiApplication.Interfaces.Services;

namespace CoreCodedChatbot.ApiApplication.Commands.Playlist
{
    public class RemoveSuperVipCommand : IRemoveSuperVipCommand
    {
        private readonly IRemoveSuperVipRepository _removeSuperVipRepository;
        private readonly IVipService _vipService;

        public RemoveSuperVipCommand(
            IRemoveSuperVipRepository removeSuperVipRepository,
            IVipService vipService
            )
        {
            _removeSuperVipRepository = removeSuperVipRepository;
            _vipService = vipService;
        }

        public async Task Remove(string username)
        {
            _removeSuperVipRepository.Remove(username);

            await _vipService.UpdateClientVips(username).ConfigureAwait(false);
        }
    }
}