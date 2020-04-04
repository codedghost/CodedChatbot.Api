using CoreCodedChatbot.ApiApplication.Commands.Playlist;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Playlist;
using CoreCodedChatbot.ApiApplication.Interfaces.Services;
using CoreCodedChatbot.ApiContract.Enums.Playlist;
using Moq;
using NUnit.Framework;

namespace CoreCodedChatbot.ApiTests.Commands.Playlist
{
    [TestFixture]
    public class PromoteUsersRegularRequestCommandTests
    {
        private Mock<IPromoteUserRequestRepository> _promoteUserRequestRepository;
        private Mock<IVipService> _vipService;

        private PromoteUsersRegularRequestCommand _subject;

        [SetUp]
        public void SetUp()
        {
            _promoteUserRequestRepository = new Mock<IPromoteUserRequestRepository>();
            _vipService = new Mock<IVipService>();
        }

        private void SetUserHasVip(bool hasVip)
        {
            _vipService.Setup(v => v.UseVip(It.IsAny<string>())).Returns(hasVip);
        }

        private void SetReturnSongId(int songId)
        {
            _promoteUserRequestRepository.Setup(p => p.PromoteUserRequest(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(songId);
        }

        private void SetUpSubject()
        {
            _subject = new PromoteUsersRegularRequestCommand(_promoteUserRequestRepository.Object, _vipService.Object);
        }

        [TestCase(false, 50, PromoteRequestResult.NoVipAvailable, TestName = "NoVipAvailableReturned_WhenUserHasNoVips")]
        [TestCase(false, 0, PromoteRequestResult.NoVipAvailable, TestName = "NoVipAvailableReturned_WhenUserHasNoVips")]
        [TestCase(true, 0, PromoteRequestResult.UnSuccessful, TestName = "UnSuccessfulReturned_WhenReturnedSongRequestIdIsInvalid")]
        [TestCase(true, -1, PromoteRequestResult.UnSuccessful, TestName = "UnSuccessfulReturned_WhenReturnedSongRequestIdIsInvalid")]
        [TestCase(true, 50, PromoteRequestResult.Successful, TestName = "SuccessfulReturned_WhenReturnedSongRequestIdIsVvalid")]
        public void Test(bool userHasVip, int returnedRequestId, PromoteRequestResult expectedResult)
        {
            SetUserHasVip(userHasVip);
            SetReturnSongId(returnedRequestId);

            SetUpSubject();

            var result = _subject.PromoteUsersRegularRequest(It.IsAny<string>(), It.IsAny<int>());

            Assert.AreEqual(expectedResult, result.PromoteRequestResult);
        }
    }
}