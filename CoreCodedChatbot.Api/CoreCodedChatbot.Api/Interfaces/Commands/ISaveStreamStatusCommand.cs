using CoreCodedChatbot.ApiContract.RequestModels.StreamStatus;

namespace CoreCodedChatbot.Api.Interfaces.Commands
{
    public interface ISaveStreamStatusCommand
    {
        bool Save(PutStreamStatusRequest request);
    }
}