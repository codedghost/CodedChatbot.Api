using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Interfaces.Queries.Search;
using CoreCodedChatbot.ApiApplication.Interfaces.Services;
using CoreCodedChatbot.ApiApplication.Models.Solr;
using CoreCodedChatbot.ApiContract.ResponseModels.Search.ChildModels;
using SolrNet;
using SolrNet.Commands.Parameters;

namespace CoreCodedChatbot.ApiApplication.Services
{
    public class SolrService : ISolrService
    {
        private readonly ISolrOperations<SongSearch> _songSearchOperations;
        private readonly IGetSongsFromSearchResultsQuery _getSongsFromSearchResultsQuery;

        public SolrService(
            ISolrOperations<SongSearch> songSearchOperations,
            IGetSongsFromSearchResultsQuery getSongsFromSearchResultsQuery
            )
        {
            _songSearchOperations = songSearchOperations;
            _getSongsFromSearchResultsQuery = getSongsFromSearchResultsQuery;
        }

        public async Task<List<BasicSongSearchResult>> Search(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return null;

            var terms = GetStringFuzzySearchTerms(input);

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

            var result = await _songSearchOperations.QueryAsync(query, new QueryOptions{ Rows = 50 });

            var resultList = result.ToList();

            var basicSongSearchResults = _getSongsFromSearchResultsQuery.Get(resultList);

            return basicSongSearchResults;
        }

        public async Task<List<BasicSongSearchResult>> Search(string artist, string songName)
        {
            if (string.IsNullOrWhiteSpace(artist) && string.IsNullOrWhiteSpace(songName))
                return null;

            var songTerms = GetStringFuzzySearchTerms(songName);
            var artistTerms = GetStringFuzzySearchTerms(artist);

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

            var resultList = result.ToList();

            var getBasicSongSearchResults= _getSongsFromSearchResultsQuery.Get(resultList);

            return getBasicSongSearchResults;
        }

        private static List<string> GetStringFuzzySearchTerms(string searchTerm)
        {
            return string.IsNullOrWhiteSpace(searchTerm) ? null : searchTerm.Split(" ").Select(s => $"{s}~2").ToList();
        }
    }
}