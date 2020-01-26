using CoreCodedChatbot.ApiContract.RequestModels.StreamStatus;

namespace CoreCodedChatbot.Api.Interfaces.Repositories.StreamStatus
{
    public interface ISaveStreamStatusRepository
    {
        bool Save(PutStreamStatusRequest request);
    }
}