using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Playlist;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Playlist;

namespace CoreCodedChatbot.ApiApplication.Commands.Playlist
{
    public class RemoveSuperVipCommand : IRemoveSuperVipCommand
    {
        private readonly IRemoveSuperVipRepository _removeSuperVipRepository;

        public RemoveSuperVipCommand(
            IRemoveSuperVipRepository removeSuperVipRepository
            )
        {
            _removeSuperVipRepository = removeSuperVipRepository;
        }

        public void Remove(string username)
        {
            _removeSuperVipRepository.Remove(username);
        }
    }
}