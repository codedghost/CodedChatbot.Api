using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Search;
using CoreCodedChatbot.ApiApplication.Models.Solr;
using CoreCodedChatbot.ApiContract.ResponseModels.Search.ChildModels;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;
using Microsoft.EntityFrameworkCore;

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

        public async Task<List<BasicSongSearchResult>> Get(List<SongSearch> searchResults)
        {
            var results = new List<BasicSongSearchResult>();
            using (var context = _chatbotContextFactory.Create())
            {
                foreach (var result in searchResults)
                {
                    var song = await context.Songs.Include(s => s.Urls)
                        .FirstOrDefaultAsync(s => s.SongId == result.SongId).ConfigureAwait(false);

                    if (song == null) continue;

                    results.Add(new BasicSongSearchResult
                    {
                        SongId = song.SongId,
                        SongName = song.SongName,
                        ArtistName = song.SongArtist,
                        CharterUsername = song.UploaderUsername,
                        Instruments = song.ChartedPaths?.Split(",").ToList(),
                        IsOfficial = song.IsOfficial,
                        IsLinkDead = !song.Urls.All(u => u.Url.StartsWith("http")),
                        IsDownloaded = false
                    });
                }
            }

            return results;
        }
    }
}