using CoreCodedChatbot.ApiApplication.Commands.StreamStatus;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.StreamStatus;
using CoreCodedChatbot.ApiContract.RequestModels.StreamStatus;
using Moq;
using NUnit.Framework;

namespace CoreCodedChatbot.ApiTests.Commands.StreamStatus
{
    [TestFixture]
    public class SaveStreamStatusCommandTests
    {
        private Mock<ISaveStreamStatusRepository> _saveStreamStatusRepository;

        private bool _successValue;

        private SaveStreamStatusCommand _subject;

        [SetUp]
        public void SetUp()
        {
            _saveStreamStatusRepository = new Mock<ISaveStreamStatusRepository>();

            _saveStreamStatusRepository.Setup(s => s.Save(It.IsAny<PutStreamStatusRequest>())).Returns(_successValue);

            _subject = new SaveStreamStatusCommand(_saveStreamStatusRepository.Object);
        }

        [Test]
        public void SuccessWhen_ValueIsReturned_RepositoryCalled()
        {
            var putRequest = It.IsAny<PutStreamStatusRequest>();

            var result = _subject.Save(putRequest);

            _saveStreamStatusRepository.Verify(s => s.Save(putRequest), Times.Once);

            Assert.AreEqual(_successValue, result);
        }
    }
}