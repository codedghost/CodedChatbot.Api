using AutoFixture.NUnit3;
using CoreCodedChatbot.ApiApplication.Commands.Playlist;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Playlist;
using Moq;
using NUnit.Framework;

namespace CoreCodedChatbot.ApiTests.Commands.Playlist
{
    [TestFixture]
    public class EditSuperVipCommandTests
    {
        private Mock<IEditSuperVipRequestRepository> _editSuperVipRequestRepository;

        private EditSuperVipCommand _subject;

        [SetUp]
        public void SetUp()
        {
            _editSuperVipRequestRepository = new Mock<IEditSuperVipRequestRepository>();

            _subject = new EditSuperVipCommand(_editSuperVipRequestRepository.Object);
        }

        [Test, AutoData]
        public void EnsureThat_RepositoryIsCalled(string username, string newText, int songId)
        {
            _editSuperVipRequestRepository.Setup(e => e.Edit(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(songId);

            var result = _subject.Edit(username, newText);

            _editSuperVipRequestRepository.Verify(e => e.Edit(username, newText));

            Assert.AreEqual(songId, result);
        }
    }
}