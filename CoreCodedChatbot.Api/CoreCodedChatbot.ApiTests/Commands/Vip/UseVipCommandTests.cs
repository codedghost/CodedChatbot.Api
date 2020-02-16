using AutoFixture.NUnit3;
using CoreCodedChatbot.ApiApplication.Commands.Vip;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Vip;
using Moq;
using NUnit.Framework;

namespace CoreCodedChatbot.ApiTests.Commands.Vip
{
    [TestFixture]
    public class UseVipCommandTests
    {
        private Mock<IUseVipRepository> _useVipRepository;

        private UseVipCommand _subject;

        [SetUp]
        public void SetUp()
        {
            _useVipRepository = new Mock<IUseVipRepository>();

            _subject = new UseVipCommand(_useVipRepository.Object);
        }

        [Test, AutoData]
        public void EnsureRepositoryIsCalled(string username, int vips)
        {
            _subject.UseVip(username, vips);

            _useVipRepository.Verify(s => s.UseVip(username, vips), Times.Once);
        }
    }
}