using System;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Repositories.Abstractions;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;

namespace CoreCodedChatbot.ApiApplication.Repositories.Quotes;

public class QuotesRepository : BaseRepository<Quote>
{
    public QuotesRepository(IChatbotContextFactory chatbotContextFactory) 
        : base(chatbotContextFactory)
    {
    }

    public async Task Archive(int quoteId, string username, bool isMod)
    {
        var quote = await GetByIdAsync(quoteId);

        CheckCanModify(quote, quoteId, username, isMod);

        quote.Enabled = false;
        quote.LastEditedBy = username;
        quote.LastEdited = DateTime.Now;

        await Context.SaveChangesAsync();
    }

    public async Task Edit(int quoteId, string quoteText, string username, bool isMod)
    {
        var quote = await GetByIdAsync(quoteId);

        CheckCanModify(quote, quoteId, username, isMod);

        quote.QuoteText = quoteText;

        quote.LastEdited = DateTime.Now;
        quote.LastEditedBy = username;

        await Context.SaveChangesAsync();
    }

    private void CheckCanModify(Quote? quote, int quoteId, string username, bool isMod)
    {
        if (!isMod && !string.Equals(quote.Username, username, StringComparison.InvariantCultureIgnoreCase))
            throw new UnauthorizedAccessException(
                $"This user cannot edit this quote as it does not belong to them. Owner: {quote.Username}, Editor: {username}");

        if (!quote.Enabled)
            throw new UnauthorizedAccessException("This quote is disabled and therefore cannot be changed");
    }
}