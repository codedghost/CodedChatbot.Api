using System.Linq;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Models.Intermediates;
using CoreCodedChatbot.ApiApplication.Repositories.Abstractions;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;
using Microsoft.EntityFrameworkCore;

namespace CoreCodedChatbot.ApiApplication.Repositories.Search;

public class GetSongBySearchIdRepository : BaseRepository<Song>
{
    public GetSongBySearchIdRepository(
        IChatbotContextFactory chatbotContextFactory
    ) : base(chatbotContextFactory)
    {
    }

    public async Task<SongSearchIntermediate> Get(int songId)
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
}