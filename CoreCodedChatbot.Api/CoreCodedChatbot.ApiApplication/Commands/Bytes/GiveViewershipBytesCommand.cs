using System.Collections.Generic;
using System.Linq;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Bytes;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Bytes;
using CoreCodedChatbot.ApiApplication.Models.Intermediates;

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

        public void Give(ChattersIntermediate chatters)
        {
            var usernames = new List<string>();

            usernames.AddRange(chatters.Broadcasters);
            usernames.AddRange(chatters.Mods);
            usernames.AddRange(chatters.Staff);
            usernames.AddRange(chatters.Admins);
            usernames.AddRange(chatters.GlobalMods);
            usernames.AddRange(chatters.Vips);
            usernames.AddRange(chatters.Viewers);

            _giveViewershipBytesRepository.Give(usernames);
        }
    }
}