using AutoFixture.NUnit3;
using CoreCodedChatbot.ApiApplication.Commands.GuessingGame;
using CoreCodedChatbot.ApiApplication.Constants;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Settings;
using Moq;
using NUnit.Framework;

namespace CoreCodedChatbot.ApiTests.Commands.GuessingGame
{
    [TestFixture]
    public class SetGuessingGameStateCommandTests
    {
        private Mock<ISetOrCreateSettingRepository> _setOrCreateSettingRepository;

        private SetGuessingGameStateCommand _subject;

        [SetUp]
        public void Setup()
        {
            _setOrCreateSettingRepository = new Mock<ISetOrCreateSettingRepository>();

            _subject = new SetGuessingGameStateCommand(_setOrCreateSettingRepository.Object);
        }

        [Test, AutoData]
        public void SuccessWhen_SettingRepositoryCalledWithCorrectValue(bool value)
        {
            var text = value.ToString().ToLower();

            _subject.Set(value);

            _setOrCreateSettingRepository.Verify(s => s.Set(SettingsKeys.GuessingGameStateSettingKey, text));
        }
    }
}