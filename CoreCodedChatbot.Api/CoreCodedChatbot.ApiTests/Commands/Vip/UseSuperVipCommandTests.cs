using AutoFixture.NUnit3;
using CoreCodedChatbot.ApiApplication.Commands.Vip;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Vip;
using CoreCodedChatbot.Config;
using Moq;
using NUnit.Framework;

namespace CoreCodedChatbot.ApiTests.Commands.Vip
{
    [TestFixture]
    public class UseSuperVipCommandTests
    {
        private Mock<IUseSuperVipRepository> _useSuperVipRepository;
        private Mock<IConfigService> _configService;

        private int _superVipCost = 30;

        private UseSuperVipCommand _subject;

        [SetUp]
        public void Setup()
        {
            _useSuperVipRepository = new Mock<IUseSuperVipRepository>();
            _configService = new Mock<IConfigService>();

            _configService.Setup(s => s.Get<int>("SuperVipCost")).Returns(_superVipCost);

            _subject = new UseSuperVipCommand(_useSuperVipRepository.Object, _configService.Object);
        }

        [Test, AutoData]
        public void EnsureRepositoryIsCalledCorrectly(string username)
        {
            _subject.UseSuperVip(username);

            _configService.Verify(s => s.Get<int>("SuperVipCost"), Times.Once);
            _useSuperVipRepository.Verify(s => s.UseSuperVip(username, _superVipCost, 1));
        }
    }
}