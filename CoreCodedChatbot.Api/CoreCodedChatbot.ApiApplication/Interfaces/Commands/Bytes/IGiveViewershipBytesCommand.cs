using System.Collections.Generic;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Commands.Bytes
{
    public interface IGiveViewershipBytesCommand
    {
        void Give(List<string> chatters);
    }
}