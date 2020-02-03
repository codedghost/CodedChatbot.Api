using CoreCodedChatbot.Api.Interfaces.Commands.Quote;
using CoreCodedChatbot.Api.Interfaces.Queries.Quote;
using CoreCodedChatbot.Api.Interfaces.Services;
using CoreCodedChatbot.Api.Intermediates;

namespace CoreCodedChatbot.Api.Services
{
    public class QuoteService : IQuoteService
    {
        private readonly IAddQuoteCommand _addQuoteCommand;
        private readonly IEditQuoteCommand _editQuoteCommand;
        private readonly IRemoveQuoteCommand _removeQuoteCommand;
        private readonly IGetQuoteQuery _getQuoteQuery;
        private readonly IGetRandomQuoteQuery _getRandomQuoteQuery;

        public QuoteService(
            IAddQuoteCommand addQuoteCommand,
            IEditQuoteCommand editQuoteCommand,
            IRemoveQuoteCommand removeQuoteCommand,
            IGetQuoteQuery getQuoteQuery,
            IGetRandomQuoteQuery getRandomQuoteQuery
        )
        {
            _addQuoteCommand = addQuoteCommand;
            _editQuoteCommand = editQuoteCommand;
            _removeQuoteCommand = removeQuoteCommand;
            _getQuoteQuery = getQuoteQuery;
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
    }
}