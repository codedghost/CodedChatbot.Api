using System.Collections.Generic;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Bytes;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Bytes;

namespace CoreCodedChatbot.ApiApplication.Commands.Bytes
{
    public class GiveViewershipBytesCommand : IGiveViewershipBytesCommand
    {
        private readonly IGiveViewershipBytesRepository _giveViewershipBytesRepository;

        public GiveViewershipBytesCommand(
            IGiveViewershipBytesRepository giveViewershipBytesRepository
            )
        {
            _giveViewershipBytesRepository = giveViewershipBytesRepository;
        }

        public void Give(List<string> chatters)
        {
            _giveViewershipBytesRepository.Give(chatters);
        }
    }
}