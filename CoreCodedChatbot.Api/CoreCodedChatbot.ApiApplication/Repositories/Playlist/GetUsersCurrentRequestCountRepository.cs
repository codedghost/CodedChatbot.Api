using System.Linq;
using CoreCodedChatbot.ApiApplication.Repositories.Abstractions;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;

namespace CoreCodedChatbot.ApiApplication.Repositories.Playlist;

public class GetUsersCurrentRequestCountRepository : BaseRepository<SongRequest>
{
    public GetUsersCurrentRequestCountRepository(
        IChatbotContextFactory chatbotContextFactory
    ) : base(chatbotContextFactory)
    {
    }

    public int GetUsersCurrentRequestCount(string username)
    {
        var requests = Context.SongRequests.Count(sr => !sr.Played && sr.Username == username);

        return requests;
    }
}