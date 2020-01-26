using CoreCodedChatbot.Api.Constants;
using CoreCodedChatbot.Api.Interfaces.Commands.GuessingGame;
using CoreCodedChatbot.Api.Interfaces.Repositories.Settings;

namespace CoreCodedChatbot.Api.Commands.GuessingGame
{
    public class SetGuessingGameStateCommand : ISetGuessingGameStateCommand
    {
        private readonly ISetOrCreateSettingRepository _setOrCreateSettingRepository;

        public SetGuessingGameStateCommand(
            ISetOrCreateSettingRepository setOrCreateSettingRepository
            )
        {
            _setOrCreateSettingRepository = setOrCreateSettingRepository;
        }

        public void Set(bool value)
        {
            _setOrCreateSettingRepository.Set(SettingsKeys.GuessingGameStateSettingKey, value ? "true" : "false");
        }
    }
}