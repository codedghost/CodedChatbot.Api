using System;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Interfaces.Queries.Search;
using CoreCodedChatbot.ApiApplication.Interfaces.Services;
using CoreCodedChatbot.ApiApplication.Models.Intermediates;
using CoreCodedChatbot.ApiApplication.Repositories.Search;
using CoreCodedChatbot.ApiContract.RequestModels.Search;
using CoreCodedChatbot.ApiContract.ResponseModels.Playlist.ChildModels;
using CoreCodedChatbot.ApiContract.ResponseModels.Search.ChildModels;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CoreCodedChatbot.ApiApplication.Services;

public class SearchService : IBaseService, ISearchService
{
    private readonly IGetSongBySearchIdQuery _getSongBySearchIdQuery;
    private readonly IDownloadChartService _downloadChartService;
    private readonly IServiceProvider _serviceProvider;
    private readonly IChatbotContextFactory _chatbotContextFactory;
    private readonly ILogger<SearchService> _logger;

    public SearchService(
        IGetSongBySearchIdQuery getSongBySearchIdQuery,
        IDownloadChartService downloadChartService,
        IServiceProvider serviceProvider,
        IChatbotContextFactory chatbotContextFactory,
        ILogger<SearchService> logger
    )
    {
        _getSongBySearchIdQuery = getSongBySearchIdQuery;
        _downloadChartService = downloadChartService;
        _serviceProvider = serviceProvider;
        _chatbotContextFactory = chatbotContextFactory;
        _logger = logger;
    }

    public async Task<bool> SaveSearchSynonymRequest(SaveSearchSynonymRequest request)
    {
        try
        {
            using (var repo = new SearchSynonymRequestsRepository(_chatbotContextFactory))
            {
                await repo.CreateAndSaveAsync(
                    new SearchSynonymRequest
                    {
                        SynonymRequest = request.SearchSynonymRequest,
                        Username = request.Username
                    });
            }

            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error when saving synonym");
            return false;
        }
    }

    public async Task DownloadSongToOneDrive(int requestSongId)
    {
        SongSearchIntermediate songRequest;
        using (var repo = new SongsRepository(_chatbotContextFactory))
        {
            songRequest = await repo.GetSongBySearchId(requestSongId).ConfigureAwait(false);
        }

        await _downloadChartService.Download(songRequest.DownloadUrl, songRequest.SongId).ConfigureAwait(false);
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