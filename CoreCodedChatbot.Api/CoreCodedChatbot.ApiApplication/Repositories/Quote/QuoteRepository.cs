using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Quote;
using CoreCodedChatbot.ApiApplication.Repositories.Abstractions;
using CoreCodedChatbot.Database.Context.Interfaces;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

namespace CoreCodedChatbot.ApiApplication.Repositories.Quote;

public class QuoteRepository : BaseRepository<Database.Context.Models.Quote>, IQuoteRepository
{
    public QuoteRepository(IChatbotContextFactory chatbotContextFactory) : base(chatbotContextFactory)
    {
    }

    public async Task Archive(int quoteId, string username, bool isMod)
    {
        var quote = await GetByIdAsync(quoteId);

        CheckCanModify(quote, quoteId, username, isMod);

        quote.Enabled = false;
        quote.LastEditedBy = username;
        quote.LastEdited = DateTime.Now;

        await _context.SaveChangesAsync();
    }

    public async Task Edit(int quoteId, string quoteText, string username, bool isMod)
    {
        var quote = await GetByIdAsync(quoteId);

        CheckCanModify(quote, quoteId, username, isMod);

        quote.QuoteText = quoteText;

        quote.LastEdited = DateTime.Now;
        quote.LastEditedBy = username;

        await _context.SaveChangesAsync();
    }

    private void CheckCanModify(Database.Context.Models.Quote? quote, int quoteId, string username, bool isMod)
    {
        if (!isMod && !string.Equals(quote.Username, username, StringComparison.InvariantCultureIgnoreCase))
            throw new UnauthorizedAccessException(
                $"This user cannot edit this quote as it does not belong to them. Owner: {quote.Username}, Editor: {username}");

        if (!quote.Enabled)
            throw new UnauthorizedAccessException("This quote is disabled and therefore cannot be changed");
    }
}