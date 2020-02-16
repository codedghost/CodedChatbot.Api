using System;
using AutoFixture;
using AutoFixture.NUnit3;
using CoreCodedChatbot.ApiApplication.Commands.GuessingGame;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.GuessingGame;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.GuessingGame;
using Moq;
using NUnit.Framework;

namespace CoreCodedChatbot.ApiTests.Commands.GuessingGame
{
    [TestFixture]
    public class SubmitOrUpdateGuessCommandTests
    {
        private Mock<ISubmitOrUpdateGuessRepository> _submitOrUpdateGuessRepository;
        private Mock<IGetRunningGuessingGameIdRepository> _getRunningGuessingGameIdRepository;

        private SubmitOrUpdateGuessCommand _subject;

        [SetUp]
        public void Setup()
        {
            _submitOrUpdateGuessRepository = new Mock<ISubmitOrUpdateGuessRepository>();
            _getRunningGuessingGameIdRepository = new Mock<IGetRunningGuessingGameIdRepository>();
        }

        private void SetCurrentGameId(int currentGameId)
        {
            _getRunningGuessingGameIdRepository.Setup(s => s.Get()).Returns(currentGameId);
        }

        private void SetupSubject()
        {
            _subject = new SubmitOrUpdateGuessCommand(_submitOrUpdateGuessRepository.Object,
                _getRunningGuessingGameIdRepository.Object);
        }

        [Test, AutoData]
        public void ThrowsExceptionWhen_NoGuessingGameId(string username, decimal percentageGuess)
        {
            SetCurrentGameId(0);
            SetupSubject();

            Assert.Throws(typeof(Exception), () => _subject.SubmitOrUpdate(username, percentageGuess));
            _getRunningGuessingGameIdRepository.Verify(s => s.Get(), Times.Once);
            _submitOrUpdateGuessRepository.Verify(
                s => s.Submit(It.IsAny<int>(), username, percentageGuess), Times.Never);
        }

        [Test, AutoData]
        public void SuccessfulWhen_GuessingGameIdReturns(string username, decimal percentageGuess)
        {
            var fixture = new Fixture();
            fixture.Customizations.Add(new RandomNumericSequenceGenerator(1, int.MaxValue));

            var currentGameId = fixture.Create<int>();

            SetCurrentGameId(currentGameId);
            SetupSubject();

            _subject.SubmitOrUpdate(username, percentageGuess);

            _getRunningGuessingGameIdRepository.Verify(s => s.Get(), Times.Once);
            _submitOrUpdateGuessRepository.Verify(s => s.Submit(currentGameId, username, percentageGuess), Times.Once);
        }
    }
}