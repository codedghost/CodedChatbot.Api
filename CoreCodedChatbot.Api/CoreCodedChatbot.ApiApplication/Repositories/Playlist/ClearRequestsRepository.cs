using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Models.Intermediates;
using CoreCodedChatbot.ApiApplication.Repositories.Abstractions;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;

namespace CoreCodedChatbot.ApiApplication.Repositories.Playlist;

public class ClearRequestsRepository : BaseRepository<SongRequest>
{
    private readonly IChatbotContextFactory _chatbotContextFactory;

    public ClearRequestsRepository(
        IChatbotContextFactory chatbotContextFactory
    ) : base(chatbotContextFactory)
    {
        _chatbotContextFactory = chatbotContextFactory;
    }

    public async Task ClearRequests(List<BasicSongRequest> requestsToRemove)
    {
        if (requestsToRemove == null || !requestsToRemove.Any()) return;

        foreach (var request in requestsToRemove)
        {
            var songRequest = await GetByIdAsync(request.SongRequestId);

            songRequest.Played = true;
        }

        await Context.SaveChangesAsync();
    }
}