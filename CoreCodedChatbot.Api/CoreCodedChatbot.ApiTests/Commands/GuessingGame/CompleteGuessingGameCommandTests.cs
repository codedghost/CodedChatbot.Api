using AutoFixture.NUnit3;
using CoreCodedChatbot.ApiApplication.Commands.GuessingGame;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.GuessingGame;
using Moq;
using NUnit.Framework;

namespace CoreCodedChatbot.ApiTests
{
    [TestFixture]
    public class CompleteGuessingGameCommandTests
    {
        private Mock<ICompleteGuessingGameRepository> _completeGuessingGameRepository;

        private CompleteGuessingGameCommand _subject;

        [SetUp]
        public void SetUp()
        {
            _completeGuessingGameRepository = new Mock<ICompleteGuessingGameRepository> ();
            _completeGuessingGameRepository.Setup(s => s.CompleteCurrentGuessingGame(It.IsAny<decimal>()));

            _subject = new CompleteGuessingGameCommand(_completeGuessingGameRepository.Object);
        }

        [Test, AutoData]
        public void SuccessWhen_RepositoryIsCalled(decimal guess)
        {
            _subject.CompleteCurrentGuessingGame(guess);

            _completeGuessingGameRepository.Verify(s => s.CompleteCurrentGuessingGame(guess));
        }
    }
}