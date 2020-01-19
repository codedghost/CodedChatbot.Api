using CoreCodedChatbot.ApiContract.RequestModels.StreamStatus;

namespace CoreCodedChatbot.Api.Commands
{
    public interface ISaveStreamStatusCommand
    {
        bool Save(PutStreamStatusRequest request);
    }
}