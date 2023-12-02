using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Repositories.Abstractions;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;

namespace CoreCodedChatbot.ApiApplication.Repositories.Playlist;

public class ArchiveRequestRepository : BaseRepository<SongRequest>
{
    public ArchiveRequestRepository(
        IChatbotContextFactory chatbotContextFactory
    ) : base(chatbotContextFactory)
    {
    }

    public async Task<string> ArchiveRequest(int requestId)
    {
        var request = await GetByIdOrNullAsync(requestId);

        if (request == null) return string.Empty;

        request.Played = true;

        await Context.SaveChangesAsync();

        return request.Username;
    }
}