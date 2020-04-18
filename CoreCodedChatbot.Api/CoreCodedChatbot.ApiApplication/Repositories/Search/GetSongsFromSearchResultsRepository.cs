using System.Collections.Generic;
using System.Linq;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Search;
using CoreCodedChatbot.ApiApplication.Models.Solr;
using CoreCodedChatbot.ApiContract.ResponseModels.Search.ChildModels;
using CoreCodedChatbot.Database.Context.Interfaces;

namespace CoreCodedChatbot.ApiApplication.Repositories.Search
{
    public class GetSongsFromSearchResultsRepository : IGetSongsFromSearchResultsRepository
    {
        private readonly IChatbotContextFactory _chatbotContextFactory;

        public GetSongsFromSearchResultsRepository(
            IChatbotContextFactory chatbotContextFactory
            )
        {
            _chatbotContextFactory = chatbotContextFactory;
        }

        public List<BasicSongSearchResult> Get(List<SongSearch> searchResults)
        {
            var results = new List<BasicSongSearchResult>();
            using (var context = _chatbotContextFactory.Create())
            {
                foreach (var result in searchResults)
                {
                    var song = context.Songs.Find(result.SongId);

                    if (song == null) continue;

                    results.Add(new BasicSongSearchResult
                    {
                        SongId = song.SongId,
                        SongName = song.SongName,
                        ArtistName = song.SongArtist,
                        Instruments = song.ChartedPaths?.Split(",").ToList(),
                        IsOfficial = song.IsOfficial,
                        IsLinkDead = !song.DownloadUrl.StartsWith("http")
                    });
                }
            }

            return results;
        }
    }
}