using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Search;
using CoreCodedChatbot.ApiApplication.Interfaces.Queries.Search;
using CoreCodedChatbot.ApiApplication.Interfaces.Services;
using CoreCodedChatbot.ApiContract.RequestModels.Search;

namespace CoreCodedChatbot.ApiApplication.Services
{
    public class SearchService : ISearchService
    {
        private readonly ISaveSearchSynonymRequestCommand _saveSearchSynonymRequestCommand;
        private readonly IGetSongBySearchIdQuery _getSongBySearchIdQuery;
        private readonly IDownloadChartService _downloadChartService;

        public SearchService(
            ISaveSearchSynonymRequestCommand saveSearchSynonymRequestCommand,
            IGetSongBySearchIdQuery getSongBySearchIdQuery,
            IDownloadChartService downloadChartService
            )
        {
            _saveSearchSynonymRequestCommand = saveSearchSynonymRequestCommand;
            _getSongBySearchIdQuery = getSongBySearchIdQuery;
            _downloadChartService = downloadChartService;
        }

        public bool SaveSearchSynonymRequest(SaveSearchSynonymRequest request)
        {
            return _saveSearchSynonymRequestCommand.Save(request.SearchSynonymRequest, request.Username);
        }

        public void DownloadSongToOneDrive(int requestSongId)
        {
            var song = _getSongBySearchIdQuery.Get(requestSongId);

            _downloadChartService.Download(song.DownloadUrl, song.SongId);
        }
    }
}