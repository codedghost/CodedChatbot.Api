using System.Collections.Generic;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Quote;
using CoreCodedChatbot.ApiApplication.Interfaces.Queries.Quote;
using CoreCodedChatbot.ApiApplication.Interfaces.Services;
using CoreCodedChatbot.ApiApplication.Models.Intermediates;

namespace CoreCodedChatbot.ApiApplication.Services
{
    public class QuoteService : IQuoteService
    {
        private readonly IAddQuoteCommand _addQuoteCommand;
        private readonly IEditQuoteCommand _editQuoteCommand;
        private readonly IRemoveQuoteCommand _removeQuoteCommand;
        private readonly IGetQuoteQuery _getQuoteQuery;
        private readonly IGetQuotesQuery _getQuotesQuery;
        private readonly IGetRandomQuoteQuery _getRandomQuoteQuery;

        public QuoteService(
            IAddQuoteCommand addQuoteCommand,
            IEditQuoteCommand editQuoteCommand,
            IRemoveQuoteCommand removeQuoteCommand,
            IGetQuoteQuery getQuoteQuery,
            IGetQuotesQuery getQuotesQuery,
            IGetRandomQuoteQuery getRandomQuoteQuery
        )
        {
            _addQuoteCommand = addQuoteCommand;
            _editQuoteCommand = editQuoteCommand;
            _removeQuoteCommand = removeQuoteCommand;
            _getQuoteQuery = getQuoteQuery;
            _getQuotesQuery = getQuotesQuery;
            _getRandomQuoteQuery = getRandomQuoteQuery;
        }

        public int AddQuote(string username, string quoteText)
        {
            var quoteId = _addQuoteCommand.AddQuote(username, quoteText);

            return quoteId;
        }

        public void EditQuote(int quoteId, string quoteText, string username, bool isMod)
        {
            _editQuoteCommand.EditQuote(quoteId, quoteText, username, isMod);
        }

        public void RemoveQuote(int quoteId, string username, bool isMod)
        {
            _removeQuoteCommand.RemoveQuote(quoteId, username, isMod);
        }

        public QuoteIntermediate GetQuote(int? quoteId)
        {
            return quoteId == null ? _getRandomQuoteQuery.GetRandomQuote() : _getQuoteQuery.GetQuote(quoteId.Value);
        }

        public List<QuoteIntermediate> GetQuotes()
        {
            return _getQuotesQuery.Get();
        }
    }
}