using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Search;
using CoreCodedChatbot.ApiApplication.Models.Solr;
using CoreCodedChatbot.ApiContract.ResponseModels.Search.ChildModels;
using CoreCodedChatbot.Database.Context.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CoreCodedChatbot.ApiApplication.Repositories.Search
{
    public class GetPriorityChartFromSearchResultsRepository : IGetPriorityChartFromSearchResultsRepository
    {
        private readonly IChatbotContextFactory _chatbotContextFactory;

        public GetPriorityChartFromSearchResultsRepository(
            IChatbotContextFactory chatbotContextFactory)
        {
            _chatbotContextFactory = chatbotContextFactory;
        }

        public async Task<BasicSongSearchResult> Get(List<SongSearch> solrResults)
        {
            using (var context = _chatbotContextFactory.Create())
            {
                var ids = solrResults.Select(r => r.SongId);
                var query = context.Songs
                    .Include(s => s.Urls)
                    .Include(s => s.Charter)
                    .Where(s => ids.Contains(s.SongId));

                if (query.Count(s => s.Charter.Preferred) > 0)
                {
                    query = query.Where(s => s.Charter.Preferred);
                }

                var song = await query.OrderByDescending(s => s.UpdatedDateTime).FirstOrDefaultAsync()
                    .ConfigureAwait(false);

                return new BasicSongSearchResult
                {
                    SongId = song.SongId,
                    SongName = HttpUtility.HtmlDecode(song.SongName),
                    ArtistName = HttpUtility.HtmlDecode(song.SongArtist),
                    CharterUsername = HttpUtility.HtmlDecode(song.Charter.Name),
                    Instruments = song.ChartedPaths?.Split(",").ToList(),
                    IsOfficial = song.IsOfficial,
                    IsLinkDead = !song.Urls.All(u => u.Url.StartsWith("http")),
                    IsDownloaded = false,
                    DownloadUrl = song.Urls.OrderByDescending(u => u.Version).FirstOrDefault()?.Url
                };
            }
        }
    }
}