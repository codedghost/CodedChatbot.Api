using AutoFixture.NUnit3;
using CoreCodedChatbot.ApiApplication.Commands.Playlist;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Playlist;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Playlist;
using Moq;
using NUnit.Framework;

namespace CoreCodedChatbot.ApiTests.Commands.Playlist
{
    [TestFixture]
    public class AddSongToDriveCommandTests
    {
        private Mock<IAddSongToDriveRepository> _addSongToDriveRepository;

        private AddSongToDriveCommand _subject;

        [SetUp]
        public void SetUp()
        {
            _addSongToDriveRepository = new Mock<IAddSongToDriveRepository>();

            _addSongToDriveRepository.Setup(a => a.AddSongToDrive(It.IsAny<int>())).Returns(true);

            _subject = new AddSongToDriveCommand(_addSongToDriveRepository.Object);
        }

        [TestCase(50, true, TestName = "SuccessWhen_ValidSongId")]
        [TestCase(0, false, TestName = "FailureWhen_InvalidSongId")]
        [TestCase(-1, false, TestName = "FailureWhen_InvalidSongId")]
        public void Tests(int songId, bool expectedResult)
        {
            var result = _subject.AddSongToDrive(songId);

            Assert.AreEqual(expectedResult, result);
        }

        [Test, AutoData]
        public void EnsureThat_AddSongToDriveRepositoryIsCalled_WhenValidSongId(int songRequestId)
        {
            _subject.AddSongToDrive(songRequestId);

            _addSongToDriveRepository.Verify(a => a.AddSongToDrive(songRequestId), Times.Once);
        }

        [TestCase(0, TestName = "EnsureThat_AddSongToDriveRepositoryIsNotCalled_WhenInvalidSongId")]
        [TestCase(-1, TestName = "EnsureThat_AddSongToDriveRepositoryIsNotCalled_WhenInvalidSongId")]
        public void InvalidSongTest(int songRequestId)
        {
            _subject.AddSongToDrive(songRequestId);

            _addSongToDriveRepository.Verify(a => a.AddSongToDrive(songRequestId), Times.Never);
        }
    }
}