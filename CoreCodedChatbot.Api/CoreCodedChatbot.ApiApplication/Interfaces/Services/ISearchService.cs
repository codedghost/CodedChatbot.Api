using CoreCodedChatbot.ApiContract.RequestModels.Search;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Services
{
    public interface ISearchService
    {
        bool SaveSearchSynonymRequest(SaveSearchSynonymRequest request);
        void DownloadSongToOneDrive(int requestSongId);
    }
}