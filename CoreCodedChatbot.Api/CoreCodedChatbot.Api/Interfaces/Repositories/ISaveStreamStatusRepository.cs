using CoreCodedChatbot.ApiContract.RequestModels.StreamStatus;

namespace CoreCodedChatbot.Api.Interfaces.Repositories
{
    public interface ISaveStreamStatusRepository
    {
        bool Save(PutStreamStatusRequest request);
    }
}