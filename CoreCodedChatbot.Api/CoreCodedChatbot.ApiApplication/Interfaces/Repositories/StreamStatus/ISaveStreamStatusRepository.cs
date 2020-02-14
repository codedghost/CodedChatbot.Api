using CoreCodedChatbot.ApiContract.RequestModels.StreamStatus;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Repositories.StreamStatus
{
    public interface ISaveStreamStatusRepository
    {
        bool Save(PutStreamStatusRequest request);
    }
}