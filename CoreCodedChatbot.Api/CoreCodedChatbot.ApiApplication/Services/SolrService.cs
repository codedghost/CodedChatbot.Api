using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Extensions;
using CoreCodedChatbot.ApiApplication.Interfaces.Queries.Search;
using CoreCodedChatbot.ApiApplication.Interfaces.Services;
using CoreCodedChatbot.ApiApplication.Models.Solr;
using CoreCodedChatbot.ApiApplication.Repositories.Search;
using CoreCodedChatbot.ApiContract.ResponseModels.Search.ChildModels;
using CoreCodedChatbot.Database.Context.Interfaces;
using SolrNet;
using SolrNet.Commands.Parameters;

namespace CoreCodedChatbot.ApiApplication.Services;

public class SolrService : IBaseService, ISolrService
{
    private readonly ISolrOperations<SongSearch> _songSearchOperations;
    private readonly IGetSongsFromSearchResultsQuery _getSongsFromSearchResultsQuery;
    private readonly IChatbotContextFactory _chatbotContextFactory;

    public SolrService(
        ISolrOperations<SongSearch> songSearchOperations,
        IGetSongsFromSearchResultsQuery getSongsFromSearchResultsQuery,
        IChatbotContextFactory chatbotContextFactory
    )
    {
        _songSearchOperations = songSearchOperations;
        _getSongsFromSearchResultsQuery = getSongsFromSearchResultsQuery;
        _chatbotContextFactory = chatbotContextFactory;
    }

    public async Task<List<BasicSongSearchResult>> Search(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return null;

        var terms = GetStringSearchTerms(input);

        AbstractSolrQuery query = null;

        foreach (var term in terms)
        {
            if (query == null)
            {
                query = new SolrQueryByField(SolrSearchConstants.SongName, term) { Quoted = false };
            }
            else
            {
                query = query || new SolrQueryByField(SolrSearchConstants.SongName, term) { Quoted = false };
            }

            query = query || new SolrQueryByField(SolrSearchConstants.ArtistName, term) { Quoted = false };
        }

        var result = await _songSearchOperations.QueryAsync(query, new QueryOptions{ Rows = 50 }).ConfigureAwait(false);

        var resultList = result.ToList();

        using (var repo = new SongsRepository(_chatbotContextFactory))
        {
            return await repo.GetSongsFromSearchResults(resultList).ConfigureAwait(false);
        }
    }

    private async Task<List<SongSearch>> Search(string artist, string songName)
    {
        if (string.IsNullOrWhiteSpace(artist) && string.IsNullOrWhiteSpace(songName))
            return null;

        var songTerms = GetStringSearchTerms(songName);
        var artistTerms = GetStringSearchTerms(artist);
            
        var songQuery = new SolrQueryInList(SolrSearchConstants.SongName, songTerms) { Quoted = false };
        var artistQuery = new SolrQueryInList(SolrSearchConstants.ArtistName, artistTerms) { Quoted = false };
        AbstractSolrQuery query;

        if (songTerms != null && artistTerms != null)
        {
            query = songQuery && artistQuery;
        }
        else if (songTerms != null && artistTerms == null)
        {
            query = songQuery;
        }
        else
        {
            query = artistQuery;
        }

        var result = await _songSearchOperations.QueryAsync(query);

        return result.ToList();
    }

    private async Task<List<SongSearch>> SearchExact(string artist, string songName)
    {
        if (string.IsNullOrWhiteSpace(artist) && string.IsNullOrWhiteSpace(songName))
            return null;

        var songQuery = GetSolrQueryForTerms(SolrSearchConstants.SongName, songName);
        var artistQuery = GetSolrQueryForTerms(SolrSearchConstants.ArtistName, artist);
            
        AbstractSolrQuery query;

        if (songQuery != null && artistQuery!= null)
        {
            query = songQuery && artistQuery;
        }
        else if (songQuery != null && artistQuery == null)
        {
            query = songQuery;
        }
        else
        {
            query = artistQuery;
        }

        var result = await _songSearchOperations.QueryAsync(query);

        return result.ToList();
    }

    public async Task<List<BasicSongSearchResult>> SearchWithFallback(string artist, string songName)
    {
        var solrResult = await SolrSearchWithFallback(artist, songName).ConfigureAwait(false);

        using (var repo = new SongsRepository(_chatbotContextFactory))
        {
            return await repo.GetSongsFromSearchResults(solrResult).ConfigureAwait(false);
        }
    }

    private async Task<List<SongSearch>> SolrSearchWithFallback(string artist, string songName)
    {
        var solrResult = await SearchExact(artist, songName).ConfigureAwait(false);

        if (!solrResult.Any())
        {
            solrResult = await Search(artist, songName).ConfigureAwait(false);
        }

        return solrResult;
    }

    public async Task<BasicSongSearchResult> SearchSingleWithFallback(string artist, string songName)
    {
        var exact = true;
        var solrResults = await SearchExact(artist, songName).ConfigureAwait(false);

        if (!solrResults.Any())
        {
            exact = false;
            solrResults = await Search(artist, songName).ConfigureAwait(false);
        }

        if (!solrResults.Any()) return null;

        using (var repo = new SongsRepository(_chatbotContextFactory))
        {
            var priorityChart = await repo.GetPriorityChartFromSearchResults(exact
                ? solrResults
                : new List<SongSearch>
                {
                    solrResults.First()
                }).ConfigureAwait(false);

            return priorityChart;
        }
    }

    private static List<string>? GetStringSearchTerms(string searchTerm)
    {
        return string.IsNullOrWhiteSpace(searchTerm) ? null :
            searchTerm.ReplaceSpecialChars().Split(' ', '\'')
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .ToList();
    }

    private static AbstractSolrQuery GetSolrQueryForTerms(string field, string searchTerm)
    {
        var stringList = GetStringSearchTerms(searchTerm);

        AbstractSolrQuery query = null;
        var firstRun = true;
        foreach (var term in stringList ?? new List<string>())
        {
            var newTerm = new SolrQueryByField(field, term){Quoted = false};
            if (firstRun)
            {
                query = newTerm;
                firstRun = false;
                continue;
            }

            query = query && newTerm;
        }

        return query;
    }
}