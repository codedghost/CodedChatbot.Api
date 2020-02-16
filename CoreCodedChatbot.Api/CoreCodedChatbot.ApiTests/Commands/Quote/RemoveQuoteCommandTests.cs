using CoreCodedChatbot.ApiApplication.Commands.Quote;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Quote;
using Moq;
using NUnit.Framework;

namespace CoreCodedChatbot.ApiTests.Commands.Quote
{
    [TestFixture]
    public class RemoveQuoteCommandTests
    {
        private Mock<IRemoveQuoteRepository> _removeQuoteRepository;

        private RemoveQuoteCommand _subject;

        [SetUp]
        public void SetUp()
        {
            _removeQuoteRepository = new Mock<IRemoveQuoteRepository>();

            _subject = new RemoveQuoteCommand(_removeQuoteRepository.Object);
        }

        [Test]
        public void SuccessWhen_RepositoryCalled()
        {
            var quoteId = It.IsAny<int>();
            var username = It.IsAny<string>();
            var isMod = It.IsAny<bool>();

            _subject.RemoveQuote(quoteId, username, isMod);

            _removeQuoteRepository.Verify(e => e.RemoveQuote(quoteId, username, isMod));
        }
    }
}