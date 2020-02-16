using AutoFixture.NUnit3;
using CoreCodedChatbot.ApiApplication.Commands.Quote;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Quote;
using Moq;
using NUnit.Framework;

namespace CoreCodedChatbot.ApiTests.Commands.Quote
{
    [TestFixture]
    public class EditQuoteCommandTests
    {
        private Mock<IEditQuoteRepository> _editQuoteRepository;

        private EditQuoteCommand _subject;

        [SetUp]
        public void SetUp()
        {
            _editQuoteRepository = new Mock<IEditQuoteRepository>();

            _subject = new EditQuoteCommand(_editQuoteRepository.Object);
        }

        [Test, AutoData]
        public void SuccessWhen_RepositoryCalled(int quoteId, string quoteText, string username, bool isMod)
        {
            _subject.EditQuote(quoteId, quoteText, username, isMod);

            _editQuoteRepository.Verify(e => e.EditQuote(quoteId, quoteText, username, isMod));
        }
    }
}