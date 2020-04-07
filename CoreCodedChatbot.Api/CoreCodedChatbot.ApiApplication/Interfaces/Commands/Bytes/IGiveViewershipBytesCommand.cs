using CoreCodedChatbot.ApiApplication.Models.Intermediates;

namespace CoreCodedChatbot.ApiApplication.Commands.Bytes
{
    public interface IGiveViewershipBytesCommand
    {
        void Give(ChattersIntermediate chatters);
    }
}