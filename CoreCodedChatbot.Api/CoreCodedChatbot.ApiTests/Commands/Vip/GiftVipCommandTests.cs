using AutoFixture.NUnit3;
using CoreCodedChatbot.ApiApplication.Commands.Vip;
using CoreCodedChatbot.ApiApplication.Interfaces.Queries.Vip;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Vip;
using Moq;
using NUnit.Framework;

namespace CoreCodedChatbot.ApiTests.Commands.Vip
{
    [TestFixture]
    public class GiftVipCommandTests
    {
        private Mock<IGiftVipRepository> _giftVipRepository;
        private Mock<ICheckUserHasVipsQuery> _checkUserHasVipsQuery;

        private GiftVipCommand _subject;

        [SetUp]
        public void Setup()
        {
            _giftVipRepository = new Mock<IGiftVipRepository>();
            _checkUserHasVipsQuery = new Mock<ICheckUserHasVipsQuery>();
        }

        private void SetUserHasEnoughVips(bool hasEnough)
        {
            _checkUserHasVipsQuery.Setup(s => s.CheckUserHasVips(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(hasEnough);
        }

        private void SetupSubject()
        {
            _subject = new GiftVipCommand(_giftVipRepository.Object, _checkUserHasVipsQuery.Object);
        }

        [Test, AutoData]
        public void ReturnsTrueWhen_UserHasEnoughVips(string donorUsername, string receivingUsername, int vipsToGift)
        {
            SetUserHasEnoughVips(true);
            SetupSubject();
            
            var result = _subject.GiftVip(donorUsername, receivingUsername, vipsToGift);

            _checkUserHasVipsQuery.Verify(s => s.CheckUserHasVips(donorUsername, vipsToGift), Times.Once);
            _giftVipRepository.Verify(s => s.GiftVip(donorUsername, receivingUsername, vipsToGift), Times.Once);

            Assert.IsTrue(result);
        }

        [Test, AutoData]
        public void ReturnsFalseWhen_UserDoesNotHaveEnoughVips(string donorUsername, string receivingUsername,
            int vipsToGift)
        {
            SetUserHasEnoughVips(false);
            SetupSubject();

            var result = _subject.GiftVip(donorUsername, receivingUsername, vipsToGift);

            _checkUserHasVipsQuery.Verify(s => s.CheckUserHasVips(donorUsername, vipsToGift), Times.Once);
            _giftVipRepository.Verify(s => s.GiftVip(donorUsername, receivingUsername, vipsToGift), Times.Never);

            Assert.IsFalse(result);
        }
    }
}