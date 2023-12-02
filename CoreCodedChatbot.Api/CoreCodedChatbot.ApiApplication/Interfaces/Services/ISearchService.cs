using System.Threading.Tasks;
using CoreCodedChatbot.ApiContract.RequestModels.Search;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Services;

public interface ISearchService
{
    Task<bool> SaveSearchSynonymRequest(SaveSearchSynonymRequest request);
    Task DownloadSongToOneDrive(int requestSongId);
    Task<int> FindChartAndDownload(string requestText);
}