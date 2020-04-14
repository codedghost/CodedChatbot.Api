using CoreCodedChatbot.ApiApplication.Models.Intermediates;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Commands.Bytes
{
    public interface IGiveViewershipBytesCommand
    {
        void Give(ChattersIntermediate chatters);
    }
}