using System.Collections.Generic;
using System.Linq;
using CoreCodedChatbot.ApiApplication.Commands.Playlist;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Vip;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Playlist;
using CoreCodedChatbot.ApiApplication.Models.Intermediates;
using CoreCodedChatbot.Config;
using Moq;
using NUnit.Framework;

namespace CoreCodedChatbot.ApiTests.Commands.Playlist
{
    [TestFixture]
    public class RemoveAndRefundAllRequestsCommandTests
    {
        private Mock<IGetCurrentRequestsRepository> _getCurrentRequestsRepository;
        private Mock<IClearRequestsRepository> _clearRequestsRepository;
        private Mock<IRefundVipCommand> _refundVipCommand;
        private Mock<IConfigService> _configService;

        private RemoveAndRefundAllRequestsCommand _subject;

        private List<BasicSongRequest> _nullRequests = null;
        private List<BasicSongRequest> _populatedRegularRequests;
        private List<BasicSongRequest> _populatedVipRequests;

        private int _superVipCost = 50;

        [SetUp]
        public void SetUp()
        {
            _getCurrentRequestsRepository = new Mock<IGetCurrentRequestsRepository>();
            _clearRequestsRepository = new Mock<IClearRequestsRepository>();
            _refundVipCommand = new Mock<IRefundVipCommand>();
            _configService = new Mock<IConfigService>();

            _configService.Setup(c => c.Get<int>("SuperVipCost")).Returns(_superVipCost);
        }

        private void SetUpNullResponse()
        {
            _getCurrentRequestsRepository.Setup(g => g.GetCurrentRequests())
                .Returns((CurrentRequestsIntermediate) null);
        }

        private void SetUpNullRequestsResponse()
        {
            _populatedRegularRequests = null;
            _populatedVipRequests = null;
            _getCurrentRequestsRepository.Setup(g => g.GetCurrentRequests()).Returns(new CurrentRequestsIntermediate
            {
                RegularRequests = _populatedRegularRequests,
                VipRequests = _populatedVipRequests
            });
        }

        private void SetUpNoCurrentRequests()
        {
            _populatedRegularRequests = new List<BasicSongRequest>();
            _populatedVipRequests = new List<BasicSongRequest>();
            _getCurrentRequestsRepository.Setup(g => g.GetCurrentRequests()).Returns(new CurrentRequestsIntermediate
            {
                RegularRequests = _populatedRegularRequests,
                VipRequests = _populatedVipRequests
            });
        }

        private void SetUpOnlyRegularRequests()
        {
            _populatedRegularRequests = new List<BasicSongRequest>
            {
                new BasicSongRequest
                {
                    Username = It.IsAny<string>(),
                    IsVip = false,
                    IsSuperVip = false
                },
                new BasicSongRequest
                {
                    Username = It.IsAny<string>(),
                    IsVip = false,
                    IsSuperVip = false
                }
            };
            _populatedVipRequests = new List<BasicSongRequest>();

            _getCurrentRequestsRepository.Setup(g => g.GetCurrentRequests()).Returns(new CurrentRequestsIntermediate
            {
                RegularRequests = _populatedRegularRequests,
                VipRequests = _populatedVipRequests
            });
        }

        private void SetUpOnlyVips()
        {
            _populatedRegularRequests = new List<BasicSongRequest>();
            _populatedVipRequests = new List<BasicSongRequest>
            {
                new BasicSongRequest
                {
                    Username = It.IsAny<string>(),
                    IsVip = true,
                    IsSuperVip = false
                },
                new BasicSongRequest
                {
                    Username = It.IsAny<string>(),
                    IsVip = true,
                    IsSuperVip = false
                }
            };

            _getCurrentRequestsRepository.Setup(g => g.GetCurrentRequests()).Returns(new CurrentRequestsIntermediate
            {
                RegularRequests = _populatedRegularRequests,
                VipRequests = _populatedVipRequests
            });
        }

        private void SetUpOnlySuperVips()
        {
            _populatedRegularRequests = new List<BasicSongRequest>();
            _populatedVipRequests = new List<BasicSongRequest>
            {
                new BasicSongRequest
                {
                    Username = It.IsAny<string>(),
                    IsVip = true,
                    IsSuperVip = true
                },
                new BasicSongRequest
                {
                    Username = It.IsAny<string>(),
                    IsVip = true,
                    IsSuperVip = true
                }
            };

            _getCurrentRequestsRepository.Setup(g => g.GetCurrentRequests()).Returns(new CurrentRequestsIntermediate
            {
                RegularRequests = _populatedRegularRequests,
                VipRequests = _populatedVipRequests
            });
        }

        private void SetUpMixedRequests()
        {

            _populatedRegularRequests = new List<BasicSongRequest>
            {
                new BasicSongRequest
                {
                    Username = It.IsAny<string>(),
                    IsVip = false,
                    IsSuperVip = false
                },
                new BasicSongRequest
                {
                    Username = It.IsAny<string>(),
                    IsVip = false,
                    IsSuperVip = false
                }
            };
            _populatedVipRequests = new List<BasicSongRequest>
            {
                new BasicSongRequest
                {
                    Username = It.IsAny<string>(),
                    IsVip = true,
                    IsSuperVip = false
                },
                new BasicSongRequest
                {
                    Username = It.IsAny<string>(),
                    IsVip = true,
                    IsSuperVip = true
                }
            };

            _getCurrentRequestsRepository.Setup(g => g.GetCurrentRequests()).Returns(new CurrentRequestsIntermediate
            {
                RegularRequests = _populatedRegularRequests,
                VipRequests = _populatedVipRequests
            });
        }

        private void SetUpSubject()
        {
            _subject = new RemoveAndRefundAllRequestsCommand(_getCurrentRequestsRepository.Object,
                _clearRequestsRepository.Object, _refundVipCommand.Object, _configService.Object);
        }

        private bool VerifyRefundingVips(List<VipRefund> refunds)
        {
            return refunds.Sum(vip => vip.VipsToRefund) ==
                   _populatedVipRequests.Sum(vip =>
                       (vip.IsVip && !vip.IsSuperVip) ? 1 :
                       (vip.IsVip && vip.IsSuperVip) ? _superVipCost :
                       0);
        }

        [Test]
        public void GracefulExitWhen_NullCurrentRequestsResponse()
        {
            SetUpNullResponse();
            SetUpSubject();
            
            _subject.RemoveAndRefundAllRequests();

            _getCurrentRequestsRepository.Verify(g => g.GetCurrentRequests(), Times.Once);
            _refundVipCommand.Verify(r => r.Refund(It.IsAny<List<VipRefund>>()), Times.Never);
            _clearRequestsRepository.Verify(c => c.ClearRequests(It.IsAny<List<BasicSongRequest>>()), Times.Never);
        }

        [Test]
        public void GracefulExitWhen_NullRequestLists()
        {
            SetUpNullRequestsResponse();
            SetUpSubject();

            _subject.RemoveAndRefundAllRequests();

            _getCurrentRequestsRepository.Verify(g => g.GetCurrentRequests(), Times.Once);
            _refundVipCommand.Verify(r => r.Refund(new List<VipRefund>()), Times.Once);
            _clearRequestsRepository.Verify(c => c.ClearRequests(null), Times.Exactly(2));
        }

        [Test]
        public void SuccessWhen_NoCurrentRequests()
        {
            SetUpNoCurrentRequests();
            SetUpSubject();
            
            _subject.RemoveAndRefundAllRequests();

            _getCurrentRequestsRepository.Verify(g => g.GetCurrentRequests(), Times.Once);
            _refundVipCommand.Verify(r => r.Refund(new List<VipRefund>()), Times.Once);
            _clearRequestsRepository.Verify(c => c.ClearRequests(new List<BasicSongRequest>()), Times.Exactly(2));
        }

        [Test]
        public void SuccessWhen_OnlyRegularRequests()
        {
            SetUpOnlyRegularRequests();
            SetUpSubject();

            _subject.RemoveAndRefundAllRequests();

            _getCurrentRequestsRepository.Verify(g => g.GetCurrentRequests(), Times.Once);
            _refundVipCommand.Verify(r => r.Refund(new List<VipRefund>()), Times.Once);
            _clearRequestsRepository.Verify(c => c.ClearRequests(_populatedRegularRequests), Times.Once);
            _clearRequestsRepository.Verify(c => c.ClearRequests(_populatedVipRequests), Times.Once);
        }

        [Test]
        public void SuccessWhen_OnlyVipRequests()
        {
            SetUpOnlyVips();
            SetUpSubject();

            _subject.RemoveAndRefundAllRequests();

            _getCurrentRequestsRepository.Verify(g => g.GetCurrentRequests(), Times.Once);
            _refundVipCommand.Verify(r => r.Refund(It.Is((List<VipRefund> list) => VerifyRefundingVips(list))), Times.Once);
            _clearRequestsRepository.Verify(c => c.ClearRequests(_populatedRegularRequests), Times.Once);
            _clearRequestsRepository.Verify(c => c.ClearRequests(_populatedVipRequests), Times.Once);
        }

        [Test]
        public void SuccessWhen_OnlySuperVipRequests()
        {
            SetUpOnlySuperVips();
            SetUpSubject();

            _subject.RemoveAndRefundAllRequests();

            _getCurrentRequestsRepository.Verify(g => g.GetCurrentRequests(), Times.Once);
            _refundVipCommand.Verify(r => r.Refund(It.Is((List<VipRefund> list) => VerifyRefundingVips(list))), Times.Once);
            _clearRequestsRepository.Verify(c => c.ClearRequests(_populatedRegularRequests), Times.Once);
            _clearRequestsRepository.Verify(c => c.ClearRequests(_populatedVipRequests), Times.Once);
        }

        [Test]
        public void SuccessWhen_MixedRequests()
        {
            SetUpMixedRequests();
            SetUpSubject();

            _subject.RemoveAndRefundAllRequests();

            _getCurrentRequestsRepository.Verify(g => g.GetCurrentRequests(), Times.Once);
            _refundVipCommand.Verify(r => r.Refund(It.Is((List<VipRefund> list) => VerifyRefundingVips(list))), Times.Once);
            _clearRequestsRepository.Verify(c => c.ClearRequests(_populatedRegularRequests), Times.Once);
            _clearRequestsRepository.Verify(c => c.ClearRequests(_populatedVipRequests), Times.Once);
        }
    }
}