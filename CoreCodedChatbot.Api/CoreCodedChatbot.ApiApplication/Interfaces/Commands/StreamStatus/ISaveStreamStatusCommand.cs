using CoreCodedChatbot.ApiContract.RequestModels.StreamStatus;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Commands.StreamStatus
{
    public interface ISaveStreamStatusCommand
    {
        bool Save(PutStreamStatusRequest request);
    }
}