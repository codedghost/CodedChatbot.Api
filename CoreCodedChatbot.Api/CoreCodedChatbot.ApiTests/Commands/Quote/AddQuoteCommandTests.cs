using CoreCodedChatbot.ApiApplication.Commands.Quote;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Quote;
using Moq;
using NUnit.Framework;

namespace CoreCodedChatbot.ApiTests.Commands.Quote
{
    [TestFixture]
    public class AddQuoteCommandTests
    {
        private Mock<IAddQuoteRepository> _addQuoteRepository;

        private AddQuoteCommand _subject;

        [SetUp]
        public void SetUp()
        {
            _addQuoteRepository = new Mock<IAddQuoteRepository>();
            _addQuoteRepository.Setup(a => a.AddQuote(It.IsAny<string>(), It.IsAny<string>())).Returns(It.IsAny<int>());

            _subject = new AddQuoteCommand(_addQuoteRepository.Object);
        }

        [Test]
        public void SuccessWhen_RepositoryIsCalled()
        {
            var username = It.IsAny<string>();
            var text = It.IsAny<string>();

            _subject.AddQuote(username, text);

            _addQuoteRepository.Verify(a => a.AddQuote(username, text));
        }
    }
}