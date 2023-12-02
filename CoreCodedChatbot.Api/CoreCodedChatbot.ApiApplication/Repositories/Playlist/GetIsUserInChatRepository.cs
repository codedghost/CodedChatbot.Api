using System;
using System.Linq;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Repositories.Abstractions;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;

namespace CoreCodedChatbot.ApiApplication.Repositories.Playlist;

public class GetIsUserInChatRepository : BaseRepository<User>
{
    public GetIsUserInChatRepository(
        IChatbotContextFactory chatbotContextFactory
    ) : base(chatbotContextFactory)
    {
    }

    public async Task<bool> IsUserInChat(string username)
    {
        var user = await GetByIdOrNullAsync(username);

        if (user == null) return false;

        var tmiReportedInChat = user.TimeLastInChat.AddMinutes(2) >= DateTime.UtcNow;

        return tmiReportedInChat;
    }
}