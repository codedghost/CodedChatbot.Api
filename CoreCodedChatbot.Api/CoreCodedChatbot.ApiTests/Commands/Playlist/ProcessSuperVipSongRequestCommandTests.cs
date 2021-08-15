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
    public class ProcessSuperVipSongRequestCommandTests
    {
        private Mock<IIsSuperVipInQueueQuery> _isSuperVipInQueueQuery;
        private Mock<IVipService> _vipService;
        private Mock<IAddRequestRepository> _addRequestRepository;

        private ProcessSuperVipSongRequestCommand _subject;

        [SetUp]
        public void SetUp()
        {
            _isSuperVipInQueueQuery = new Mock<IIsSuperVipInQueueQuery>();
            _vipService = new Mock<IVipService>();
            _addRequestRepository = new Mock<IAddRequestRepository>();

            _addRequestRepository.Setup(a => a.AddRequest(It.IsAny<string>(), It.IsAny<string>(), false, true)).Returns(new AddSongResult
            {
                AddRequestResult = AddRequestResult.Success
            });
        }

        private void SetSuperVipInQueue(bool superInQueue)
        {
            _isSuperVipInQueueQuery.Setup(v => v.IsSuperVipInQueue()).Returns(superInQueue);
        }

        private void SetUserHasSuperVip(bool userHasSuperVip)
        {
            _vipService.Setup(v => v.UseSuperVip(It.IsAny<string>())).ReturnsAsync(userHasSuperVip);
        }

        private void SetUpSubject()
        {
            _subject = new ProcessSuperVipSongRequestCommand(
                _isSuperVipInQueueQuery.Object,
                _vipService.Object,
                _addRequestRepository.Object);
        }

        [TestCase(false, true, AddRequestResult.Success, TestName = "SuccessWhen_NoSuperVipInQueue_UserHasSuperVip")]
        [TestCase(false, false, AddRequestResult.NotEnoughVips, TestName = "FailureWhen_NoSuperVipInQueue_UserHasNoSuperVip")]
        [TestCase(true, false, AddRequestResult.OnlyOneSuper, TestName = "FailureWhen_SuperVipInQueue_UserHasNoSuperVip")]
        [TestCase(true, true, AddRequestResult.OnlyOneSuper, TestName = "FailureWhen_SuperVipInQueue_UserHasSuperVip")]
        public async Task Test(bool superVipInQueue, bool userHasSuperVip, AddRequestResult expectedResult)
        {
            SetSuperVipInQueue(superVipInQueue);
            SetUserHasSuperVip(userHasSuperVip);

            SetUpSubject();

            var result = await _subject.Process("Username", "Request Text");

            Assert.AreEqual(expectedResult, result.AddRequestResult);
        }
    }
}