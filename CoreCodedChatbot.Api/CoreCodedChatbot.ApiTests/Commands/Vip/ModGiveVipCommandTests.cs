using AutoFixture.NUnit3;
using CoreCodedChatbot.ApiApplication.Commands.Vip;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Vip;
using Moq;
using NUnit.Framework;

namespace CoreCodedChatbot.ApiTests.Commands.Vip
{
    [TestFixture]
    public class ModGiveVipCommandTests
    {
        private Mock<IModGiveVipRepository> _modGiveVipRepository;

        private ModGiveVipCommand _subject;

        [SetUp]
        public void Setup()
        {
            _modGiveVipRepository = new Mock<IModGiveVipRepository>();

            _subject = new ModGiveVipCommand(_modGiveVipRepository.Object);
        }

        [Test, AutoData]
        public void EnsureRepositoryIsCalled(string username, int vipsToGive)
        {
            _subject.ModGiveVip(username, vipsToGive);

            _modGiveVipRepository.Verify(s => s.ModGiveVip(username, vipsToGive), Times.Once);
        }
    }
}