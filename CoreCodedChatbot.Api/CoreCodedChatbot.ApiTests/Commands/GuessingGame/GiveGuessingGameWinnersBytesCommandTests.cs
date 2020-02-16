using System.Collections.Generic;
using AutoFixture.NUnit3;
using CoreCodedChatbot.ApiApplication.Commands.GuessingGame;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Bytes;
using CoreCodedChatbot.ApiApplication.Models.Intermediates;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace CoreCodedChatbot.ApiTests
{
    [TestFixture]
    public class GiveGuessingGameWinnersBytesCommandTests
    {
        private Mock<IGiveUsersBytesRepository> _giveUsersBytesRepository;

        private GiveGuessingGameWinnersBytesCommand _subject;

        [SetUp]
        public void Setup()
        {
            _giveUsersBytesRepository = new Mock<IGiveUsersBytesRepository>();

            _subject = new GiveGuessingGameWinnersBytesCommand(_giveUsersBytesRepository.Object);
        }

        [Test]
        public void RepositoryNotCalledWhen_InputIsNull()
        {
            _subject.Give(null);

            _giveUsersBytesRepository.Verify(s => s.GiveBytes(It.IsAny<List<GiveBytesToUserModel>>()), Times.Never);
        }

        [Test]
        public void RepositoryCalledWithEmptyListWhen_InputIsEmptyList()
        {
            _subject.Give(new List<GuessingGameWinner>());

            _giveUsersBytesRepository.Verify(s => s.GiveBytes(new List<GiveBytesToUserModel>()), Times.Once);
        }

        [Test, AutoData]
        public void RepositoryCalledWithPopulatedListWhen_GivenPopulatedList(
            List<GuessingGameWinner> winners)
        {
            _subject.Give(winners);

            _giveUsersBytesRepository.Verify(
                s => s.GiveBytes(It.Is<List<GiveBytesToUserModel>>(g => g.Count == winners.Count)), Times.Once);
        }
}
}