using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Repositories.Abstractions;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;

namespace CoreCodedChatbot.ApiApplication.Repositories.Playlist;

public class AddSongToDriveRepository : BaseRepository<SongRequest>
{
    public AddSongToDriveRepository(
        IChatbotContextFactory chatbotContextFactory
    ) : base(chatbotContextFactory)
    {
    }

    public async Task<bool> AddSongToDrive(int songRequestId)
    {
        var songRequest = await GetByIdOrNullAsync(songRequestId);

        if (songRequest == null || songRequest.Played || songRequest.InDrive)
            return false;

        songRequest.InDrive = true;

        await Context.SaveChangesAsync();

        return true;
    }
}