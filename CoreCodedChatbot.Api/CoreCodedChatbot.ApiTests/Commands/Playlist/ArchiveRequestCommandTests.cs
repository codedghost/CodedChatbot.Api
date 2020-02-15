using CoreCodedChatbot.ApiApplication.Commands.Playlist;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Playlist;
using Moq;
using NUnit.Framework;

namespace CoreCodedChatbot.ApiTests.Commands.Playlist
{
    [TestFixture]
    public class ArchiveRequestCommandTests
    {
        private Mock<IArchiveRequestRepository> _archiveRequestRepository;

        private ArchiveRequestCommand _subject;

        [SetUp]
        public void SetUp()
        {
            _archiveRequestRepository = new Mock<IArchiveRequestRepository>();

            _subject = new ArchiveRequestCommand(_archiveRequestRepository.Object);
        }

        [Test]
        public void EnsureRepositoryIsCalled()
        {
            var requestId = It.IsAny<int>();

            _subject.ArchiveRequest(requestId);

            _archiveRequestRepository.Verify(a => a.ArchiveRequest(requestId), Times.Once);
        }
    }
}