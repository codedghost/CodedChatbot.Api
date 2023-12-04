using CoreCodedChatbot.ApiApplication.Models.Intermediates;
using CoreCodedChatbot.ApiApplication.Models.Solr;
using CoreCodedChatbot.ApiApplication.Repositories.Abstractions;
using CoreCodedChatbot.ApiContract.ResponseModels.Search.ChildModels;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace CoreCodedChatbot.ApiApplication.Repositories.Search;

public class SongsRepository : BaseRepository<Song>
{
    public SongsRepository(IChatbotContextFactory chatbotContextFactory) : base(chatbotContextFactory)
    {
    }

    public async Task<BasicSongSearchResult> GetPriorityChartFromSearchResults(List<SongSearch> solrResults)
    {
        var ids = solrResults.Select(r => r.SongId);
        var query = Context.Songs
            .Include(s => s.Urls)
            .Include(s => s.Charter)
            .Where(s => ids.Contains(s.SongId));

        if (query.Count(s => s.Charter.Preferred) > 0)
        {
            query = query.Where(s => s.Charter.Preferred);
        }

        var song = await query
            .OrderByDescending(s => s.UpdatedDateTime)
            .FirstOrDefaultAsync()
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

    public async Task<SongSearchIntermediate> GetSongBySearchId(int songId)
    {
        var song = await Context.Songs
            .Include(s => s.Urls)
            .FirstOrDefaultAsync(s => s.SongId == songId)
            .ConfigureAwait(false);

        return new SongSearchIntermediate
        {
            SongId = song.SongId,
            SongName = song.SongName,
            SongArtist = song.SongArtist,
            DownloadUrl = song.Urls.OrderByDescending(u => u.Version).FirstOrDefault()?.Url
        };
    }
    public async Task<List<BasicSongSearchResult>> GetSongsFromSearchResults(List<SongSearch> searchResults)
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