using CoreCodedChatbot.Api.Constants;
using CoreCodedChatbot.Api.Interfaces.Repositories.GuessingGame;
using CoreCodedChatbot.Api.Interfaces.Repositories.Settings;

namespace CoreCodedChatbot.Api.Queries.GuessingGame
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