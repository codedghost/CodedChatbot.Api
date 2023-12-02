using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using CoreCodedChatbot.ApiApplication.Models.Solr;
using CoreCodedChatbot.ApiApplication.Repositories.Abstractions;
using CoreCodedChatbot.ApiContract.ResponseModels.Search.ChildModels;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;
using Microsoft.EntityFrameworkCore;

namespace CoreCodedChatbot.ApiApplication.Repositories.Search;

public class GetSongsFromSearchResultsRepository : BaseRepository<Song>
{
    public GetSongsFromSearchResultsRepository(
        IChatbotContextFactory chatbotContextFactory
    ) : base(chatbotContextFactory)
    {
    }

    public async Task<List<BasicSongSearchResult>> Get(List<SongSearch> searchResults)
    {
        var results = new List<BasicSongSearchResult>();
        foreach (var result in searchResults)
        {
            var song = await Context.Songs
                .Include(s => s.Urls)
                .Include(s => s.Charter)
                .FirstOrDefaultAsync(s => s.SongId == result.SongId).ConfigureAwait(false);

            if (song == null) continue;

            results.Add(new BasicSongSearchResult
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
            });
        }

        return results;
    }
}