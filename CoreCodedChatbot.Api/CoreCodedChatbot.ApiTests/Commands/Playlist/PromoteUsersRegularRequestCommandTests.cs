using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Commands.Playlist;
using CoreCodedChatbot.ApiApplication.Interfaces.Queries.Playlist;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Playlist;
using CoreCodedChatbot.ApiApplication.Interfaces.Services;
using CoreCodedChatbot.ApiApplication.Models.Intermediates;
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
        private Mock<IGetSongRequestByIdRepository> _getSongRequestByIdRepository;
        private Mock<IIsSuperVipInQueueQuery> _isSuperVipInQueueQuery;

        private PromoteRequestCommand _subject;

        [SetUp]
        public void SetUp()
        {
            _promoteUserRequestRepository = new Mock<IPromoteUserRequestRepository>();
            _vipService = new Mock<IVipService>();
            _getSongRequestByIdRepository = new Mock<IGetSongRequestByIdRepository>();
            _isSuperVipInQueueQuery = new Mock<IIsSuperVipInQueueQuery>();
        }

        private void SetSongRequest()
        {
            _getSongRequestByIdRepository.Setup(s => s.GetRequest(It.IsAny<int>())).Returns(new SongRequestIntermediate
            {
                IsVip = false
            });
        }
        private void SetUserHasVip(bool hasVip)
        {
            _vipService.Setup(v => v.UseVip(It.IsAny<string>())).ReturnsAsync(hasVip);
        }

        private void SetReturnSongId(int songId)
        {
            _promoteUserRequestRepository.Setup(p => p.PromoteUserRequest(It.IsAny<string>(), It.IsAny<int>(), false))
                .Returns(songId);
        }

        private void SetIsSuperVipInQueue(bool isSuperVipInQueue)
        {
            _isSuperVipInQueueQuery.Setup(p => p.IsSuperVipInQueue()).Returns(false);
        }

        private void SetUpSubject()
        {
            _subject = new PromoteRequestCommand(
                _promoteUserRequestRepository.Object,
                _vipService.Object,
                _getSongRequestByIdRepository.Object,
                _isSuperVipInQueueQuery.Object);
        }

        [TestCase(false, 50, PromoteRequestResult.NoVipAvailable, TestName = "NoVipAvailableReturned_WhenUserHasNoVips")]
        [TestCase(false, 0, PromoteRequestResult.NoVipAvailable, TestName = "NoVipAvailableReturned_WhenUserHasNoVips")]
        [TestCase(true, 0, PromoteRequestResult.UnSuccessful, TestName = "UnSuccessfulReturned_WhenReturnedSongRequestIdIsInvalid")]
        [TestCase(true, -1, PromoteRequestResult.UnSuccessful, TestName = "UnSuccessfulReturned_WhenReturnedSongRequestIdIsInvalid")]
        [TestCase(true, 50, PromoteRequestResult.Successful, TestName = "SuccessfulReturned_WhenReturnedSongRequestIdIsValid")]
        public async Task Test(bool userHasVip, int returnedRequestId, PromoteRequestResult expectedResult)
        {
            SetSongRequest();
            SetUserHasVip(userHasVip);
            SetReturnSongId(returnedRequestId);
            SetIsSuperVipInQueue(false);

            SetUpSubject();

            var result = await _subject.Promote(It.IsAny<string>(), false, It.IsAny<int>());

            Assert.AreEqual(expectedResult, result.PromoteRequestResult);
        }
    }
}