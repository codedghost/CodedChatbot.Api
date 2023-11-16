using System.Linq;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Search;
using CoreCodedChatbot.ApiApplication.Models.Intermediates;
using CoreCodedChatbot.Database.Context.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CoreCodedChatbot.ApiApplication.Repositories.Search;

public class GetSongBySearchIdRepository : IGetSongBySearchIdRepository
{
    private readonly IChatbotContextFactory _chatbotContextFactory;

    public GetSongBySearchIdRepository(
        IChatbotContextFactory chatbotContextFactory
    )
    {
        _chatbotContextFactory = chatbotContextFactory;
    }

    public async Task<SongSearchIntermediate> Get(int songId)
    {
        using (var context = _chatbotContextFactory.Create())
        {
            var song = await context.Songs.Include(s => s.Urls)
                .FirstOrDefaultAsync(s => s.SongId == songId).ConfigureAwait(false);

            return new SongSearchIntermediate
            {
                SongId = song.SongId,
                SongName = song.SongName,
                SongArtist = song.SongArtist,
                DownloadUrl = song.Urls.OrderByDescending(u => u.Version).FirstOrDefault()?.Url
            };
        }
    }
}