using CoreCodedChatbot.ApiApplication.Constants;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.GuessingGame;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Settings;

namespace CoreCodedChatbot.ApiApplication.Queries.GuessingGame
{
    public class GetGuessingGameStateQuery : IGetGuessingGameStateQuery
    {
        private readonly IGetSettingRepository _getSettingRepository;

        public GetGuessingGameStateQuery(
            IGetSettingRepository getSettingRepository
            )
        {
            _getSettingRepository = getSettingRepository;
        }

        public bool InProgress()
        {
            var gameInProgress = _getSettingRepository.Get<bool>(SettingsKeys.GuessingGameStateSettingKey);

            return gameInProgress;
        }
    }
}