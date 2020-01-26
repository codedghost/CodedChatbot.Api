using CoreCodedChatbot.ApiContract.RequestModels.StreamStatus;

namespace CoreCodedChatbot.Api.Interfaces.Commands.StreamStatus
{
    public interface ISaveStreamStatusCommand
    {
        bool Save(PutStreamStatusRequest request);
    }
}