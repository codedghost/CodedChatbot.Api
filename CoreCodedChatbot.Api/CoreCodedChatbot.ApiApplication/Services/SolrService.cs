using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Models.Solr;
using SolrNet;
using SolrNet.Commands.Parameters;

namespace CoreCodedChatbot.ApiApplication.Services
{
    public interface ISolrService
    {
        Task<List<SongSearch>> Search(string artist, string songName);
    }

    public class SolrService : ISolrService
    {
        private readonly ISolrOperations<SongSearch> _songSearchOperations;

        public SolrService(ISolrOperations<SongSearch> songSearchOperations)
        {
            _songSearchOperations = songSearchOperations;
        }

        public async Task<List<SongSearch>> Search(string artist, string songName)
        {
            if (string.IsNullOrWhiteSpace(artist) && string.IsNullOrWhiteSpace(songName))
                return null;

            var songTerms = GetStringFuzzySearchTerms(songName);
            var artistTerms= GetStringFuzzySearchTerms(artist);

            var songQuery = new SolrQueryInList("SongName", songTerms) {Quoted = false};
            var artistQuery = new SolrQueryInList("SongArtist", artistTerms) {Quoted = false};
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

            return resultList;
        }

        private static List<string> GetStringFuzzySearchTerms(string searchTerm)
        {
            return string.IsNullOrWhiteSpace(searchTerm) ? null : searchTerm.Split(" ").Select(s => $"{s}~2").ToList();
        }
    }
}