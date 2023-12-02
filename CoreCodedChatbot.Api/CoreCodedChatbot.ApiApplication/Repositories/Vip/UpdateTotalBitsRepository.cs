using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Repositories.Abstractions;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;
using CoreCodedChatbot.Database.DbExtensions;

namespace CoreCodedChatbot.ApiApplication.Repositories.Vip;

public class UpdateTotalBitsRepository : BaseRepository<User>
{
    public UpdateTotalBitsRepository(
        IChatbotContextFactory chatbotContextFactory
    ) : base(chatbotContextFactory)
    {
    }

    public async Task Update(string username, int totalBits)
    {
        var user = Context.GetOrCreateUser(username);

        user.TotalBitsDropped = totalBits;

        await Context.SaveChangesAsync();
    }
}