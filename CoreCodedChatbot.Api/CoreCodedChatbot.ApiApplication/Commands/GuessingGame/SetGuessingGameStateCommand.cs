using System.Globalization;
using CoreCodedChatbot.ApiApplication.Constants;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.GuessingGame;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Settings;

namespace CoreCodedChatbot.ApiApplication.Commands.GuessingGame
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
            _setOrCreateSettingRepository.Set(SettingsKeys.GuessingGameStateSettingKey, value.ToString().ToLower());
        }
    }
}