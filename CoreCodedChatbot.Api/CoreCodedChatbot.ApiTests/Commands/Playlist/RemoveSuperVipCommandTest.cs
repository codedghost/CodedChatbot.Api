using AutoFixture.NUnit3;
using CoreCodedChatbot.ApiApplication.Commands.Playlist;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Playlist;
using Moq;
using NUnit.Framework;

namespace CoreCodedChatbot.ApiTests.Commands.Playlist
{
    [TestFixture]
    public class RemoveSuperVipCommandTest
    {
        private Mock<IRemoveSuperVipRepository> _removeSuperRequestRepository;

        private RemoveSuperVipCommand _subject;

        [SetUp]
        public void Setup()
        {
            _removeSuperRequestRepository = new Mock<IRemoveSuperVipRepository>();

            _subject = new RemoveSuperVipCommand(_removeSuperRequestRepository.Object);
        }

        [Test, AutoData]
        public void EnsureRepositoryIsCalled(string username)
        {
            _subject.Remove(username);

            _removeSuperRequestRepository.Verify(s => s.Remove(username), Times.Once);
        }
    }
}