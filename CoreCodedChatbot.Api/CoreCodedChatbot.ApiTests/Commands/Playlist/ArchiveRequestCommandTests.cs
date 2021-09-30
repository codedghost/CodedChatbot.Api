using System.Threading.Tasks;
using AutoFixture.NUnit3;
using CoreCodedChatbot.ApiApplication.Commands.Playlist;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Vip;
using CoreCodedChatbot.ApiApplication.Interfaces.Queries.Playlist;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Playlist;
using CoreCodedChatbot.ApiApplication.Interfaces.Services;
using CoreCodedChatbot.Config;
using Moq;
using NUnit.Framework;

namespace CoreCodedChatbot.ApiTests.Commands.Playlist
{
    [TestFixture]
    public class ArchiveRequestCommandTests
    {
        private Mock<IArchiveRequestRepository> _archiveRequestRepository;
        private Mock<IVipService> _vipService;
        private Mock<IGetSongRequestByIdQuery> _getSongRequestByIdQuery;
        private Mock<IRefundVipCommand> _refundVipCommand;
        private Mock<IConfigService> _configService;

        private ArchiveRequestCommand _subject;

        [SetUp]
        public void SetUp()
        {
            _archiveRequestRepository = new Mock<IArchiveRequestRepository>();
            _getSongRequestByIdQuery = new Mock<IGetSongRequestByIdQuery>();
            _refundVipCommand = new Mock<IRefundVipCommand>();
            _vipService = new Mock<IVipService>();
            _configService = new Mock<IConfigService>();

            _subject = new ArchiveRequestCommand(_archiveRequestRepository.Object, _getSongRequestByIdQuery.Object,
                _refundVipCommand.Object, _vipService.Object, _configService.Object);
        }

        [Test, AutoData]
        public async Task EnsureRepositoryIsCalled(int requestId)
        {
            await _subject.ArchiveRequest(requestId, false).ConfigureAwait(false);

            _archiveRequestRepository.Verify(a => a.ArchiveRequest(requestId), Times.Once);
        }
    }
}