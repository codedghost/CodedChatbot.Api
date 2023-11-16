using System;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Search;
using CoreCodedChatbot.ApiApplication.Interfaces.Queries.Search;
using CoreCodedChatbot.ApiApplication.Interfaces.Services;
using CoreCodedChatbot.ApiContract.RequestModels.Search;
using CoreCodedChatbot.ApiContract.ResponseModels.Playlist.ChildModels;
using CoreCodedChatbot.ApiContract.ResponseModels.Search.ChildModels;
using Microsoft.Extensions.DependencyInjection;

namespace CoreCodedChatbot.ApiApplication.Services;

public class SearchService : ISearchService
{
    private readonly ISaveSearchSynonymRequestCommand _saveSearchSynonymRequestCommand;
    private readonly IGetSongBySearchIdQuery _getSongBySearchIdQuery;
    private readonly IDownloadChartService _downloadChartService;
    private readonly IServiceProvider _serviceProvider;

    public SearchService(
        ISaveSearchSynonymRequestCommand saveSearchSynonymRequestCommand,
        IGetSongBySearchIdQuery getSongBySearchIdQuery,
        IDownloadChartService downloadChartService,
        IServiceProvider serviceProvider
    )
    {
        _saveSearchSynonymRequestCommand = saveSearchSynonymRequestCommand;
        _getSongBySearchIdQuery = getSongBySearchIdQuery;
        _downloadChartService = downloadChartService;
        _serviceProvider = serviceProvider;
    }

    public bool SaveSearchSynonymRequest(SaveSearchSynonymRequest request)
    {
        return _saveSearchSynonymRequestCommand.Save(request.SearchSynonymRequest, request.Username);
    }

    public async Task DownloadSongToOneDrive(int requestSongId)
    {
        var song = await _getSongBySearchIdQuery.Get(requestSongId).ConfigureAwait(false);

        await _downloadChartService.Download(song.DownloadUrl, song.SongId).ConfigureAwait(false);
    }

    public async Task<int> FindChartAndDownload(string requestText)
    {
        var formattedRequest = FormattedRequest.GetFormattedRequest(requestText);

        if (formattedRequest == null) return 0;
        BasicSongSearchResult result;
        using (var scope = _serviceProvider.CreateScope())
        {
            var solrService = (SolrService) scope.ServiceProvider.GetService(typeof(ISolrService));

            result = await solrService
                .SearchSingleWithFallback(formattedRequest.SongArtist, formattedRequest.SongName)
                .ConfigureAwait(false);
        }

        if (result == null) return 0;

        await _downloadChartService.Download(result.DownloadUrl, result.SongId).ConfigureAwait(false);

        return result.SongId;
    }
}