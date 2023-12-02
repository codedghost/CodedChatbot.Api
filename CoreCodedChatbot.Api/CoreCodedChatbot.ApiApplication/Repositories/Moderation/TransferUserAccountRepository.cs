using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Repositories.Abstractions;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;
using CoreCodedChatbot.Database.DbExtensions;

namespace CoreCodedChatbot.ApiApplication.Repositories.Moderation;

public class TransferUserAccountRepository : BaseRepository<User>
{
    public TransferUserAccountRepository(IChatbotContextFactory chatbotContextFactory)
        : base(chatbotContextFactory)
    {
    }

    public async Task Transfer(string moderatorUsername, string oldUsername, string newUsername)
    {
        // All users should exist int the db at this point
        Context.TransferUser(moderatorUsername, oldUsername, newUsername);

        await Context.SaveChangesAsync();
    }
}