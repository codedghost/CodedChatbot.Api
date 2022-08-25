using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Commands.Playlist;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Playlist;
using CoreCodedChatbot.ApiApplication.Interfaces.Services;
using CoreCodedChatbot.ApiApplication.Models.Intermediates;
using CoreCodedChatbot.ApiContract.Enums.Playlist;
using Moq;
using NUnit.Framework;

namespace CoreCodedChatbot.ApiTests.Commands.Playlist
{
    [TestFixture]
    public class ProcessVipSongRequestCommandTests
    {
        private Mock<IVipService> _vipService;
        private Mock<IAddRequestRepository> _addRequestRepository;

        private ProcessVipSongRequestCommand _subject;

        [SetUp]
        public void SetUp()
        {
            _vipService = new Mock<IVipService>();
            _addRequestRepository = new Mock<IAddRequestRepository>();

            _addRequestRepository.Setup(a => a.AddRequest(It.IsAny<string>(), It.IsAny<string>(), true, false, It.IsAny<int>())).Returns(
                new AddSongResult
                {
                    AddRequestResult = AddRequestResult.Success
                });
        }

        private void SetUserHasVip(bool hasVip)
        {
            _vipService.Setup(v => v.UseVip(It.IsAny<string>())).ReturnsAsync(hasVip);
        }

        private void SetUpSubject()
        {
            _subject = new ProcessVipSongRequestCommand(_vipService.Object, _addRequestRepository.Object);
        }

        [Test]
        public async Task SuccessWhen_UserHasVip()
        {
            SetUserHasVip(true);
            SetUpSubject();

            var result = await _subject.Process("Username", "Request Text", It.IsAny<int>());

            Assert.AreEqual(AddRequestResult.Success, result.AddRequestResult);
        }

        [Test]
        public async Task FailureWhen_UserHasNoVips()
        {
            SetUserHasVip(false);
            SetUpSubject();

            var result = await _subject.Process("Username", "Request Text", It.IsAny<int>());

            Assert.AreEqual(AddRequestResult.NotEnoughVips, result.AddRequestResult);
        }
    }
}