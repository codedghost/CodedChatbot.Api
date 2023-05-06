using System;
using System.Linq;
using System.Threading.Tasks;
using CodedChatbot.TwitchFactories.Interfaces;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.GuessingGame;
using CoreCodedChatbot.ApiApplication.Interfaces.Queries.GuessingGame;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.GuessingGame;
using CoreCodedChatbot.ApiApplication.Interfaces.Services;
using CoreCodedChatbot.Config;
using Microsoft.Extensions.Logging;

namespace CoreCodedChatbot.ApiApplication.Services
{
    public class GuessingGameService : IGuessingGameService
    {
        private readonly IConfigService _configService;
        private readonly ILogger<IGuessingGameService> _logger;
        private readonly ITwitchClientFactory _twitchClientFactory;
        private readonly IOpenGuessingGameRepository _openGuessingGameRepository;
        private readonly ICloseGuessingGameRepository _closeGuessingGameRepository;
        private readonly IGetCurrentGuessingGameMetadataQuery _getCurrentGuessingGameMetadataQuery;
        private readonly ICompleteGuessingGameCommand _completeGuessingGameCommand;
        private readonly IGetPotentialWinnersQuery _getPotentialWinnersQuery;
        private readonly IGiveGuessingGameWinnersBytesCommand _giveGuessingGameWinnersBytesCommand;
        private readonly ISubmitOrUpdateGuessCommand _submitOrUpdateGuessCommand;
        private readonly IGetGuessingGameStateQuery _getGuessingGameStateQuery;
        private readonly ISetGuessingGameStateCommand _setGuessingGameStateCommand;

        public GuessingGameService(
            IConfigService configService,
            ILogger<IGuessingGameService> logger,
            ITwitchClientFactory twitchClientFactory,
            IOpenGuessingGameRepository openGuessingGameRepository,
            ICloseGuessingGameRepository closeGuessingGameRepository,
            IGetCurrentGuessingGameMetadataQuery getCurrentGuessingGameMetadataQuery,
            ICompleteGuessingGameCommand completeGuessingGameCommand,
            IGetPotentialWinnersQuery getPotentialWinnersQuery,
            IGiveGuessingGameWinnersBytesCommand giveGuessingGameWinnersBytesCommand,
            ISubmitOrUpdateGuessCommand submitOrUpdateGuessCommand,
            IGetGuessingGameStateQuery getGuessingGameStateQuery,
            ISetGuessingGameStateCommand setGuessingGameStateCommand
        )
        {
            _configService = configService;
            _logger = logger;
            _twitchClientFactory = twitchClientFactory;
            _openGuessingGameRepository = openGuessingGameRepository;
            _closeGuessingGameRepository = closeGuessingGameRepository;
            _getCurrentGuessingGameMetadataQuery = getCurrentGuessingGameMetadataQuery;
            _completeGuessingGameCommand = completeGuessingGameCommand;
            _getPotentialWinnersQuery = getPotentialWinnersQuery;
            _giveGuessingGameWinnersBytesCommand = giveGuessingGameWinnersBytesCommand;
            _submitOrUpdateGuessCommand = submitOrUpdateGuessCommand;
            _getGuessingGameStateQuery = getGuessingGameStateQuery;
            _setGuessingGameStateCommand = setGuessingGameStateCommand;
        }


        public async Task GuessingGameStart(string songName, int songLengthInSeconds)
        {
            var twitchClient = _twitchClientFactory.Get();

            var secondsForGuessingGame = _configService.Get<int>("SecondsForGuessingGame");
            var streamerChannelName = _configService.Get<string>("StreamerChannel");
            if (songLengthInSeconds < secondsForGuessingGame)
            {
                return;
            }

            if (!_openGuessingGameRepository.Open(songName))
            {
                twitchClient.SendMessage(streamerChannelName, "I couldn't start the guessing game :S");
                return;
            }

            SetGuessingGameState(true);

            twitchClient.SendMessage(streamerChannelName,
                $"The guessing game has begun! You have {secondsForGuessingGame} seconds to !guess the accuracy that {streamerChannelName} will get on {songName}!");

            await Task.Delay(TimeSpan.FromSeconds(secondsForGuessingGame));

            if (!_closeGuessingGameRepository.Close())
            {
                twitchClient.SendMessage(streamerChannelName, "I couldn't close the guessing game for some reason... SEND HALP");
            }

            twitchClient.SendMessage(streamerChannelName, "The guessing game has now closed. Good luck everyone!");

            SetGuessingGameState(false);
        }


        public bool SetPercentageAndFinishGame(decimal finalPercentage)
        {
            try
            {
                var twitchClient = _twitchClientFactory.Get();

                var currentGuessingGameMetadata = _getCurrentGuessingGameMetadataQuery.Get();

                _completeGuessingGameCommand.CompleteCurrentGuessingGame(finalPercentage);

                var streamerChannelName = _configService.Get<string>("StreamerChannel");

                var winners = _getPotentialWinnersQuery.Get(currentGuessingGameMetadata.GuessingGameRecordId,
                    finalPercentage);

                // No-one guessed?
                if (!winners.Any())
                    twitchClient.SendMessage(streamerChannelName, "Nobody guessed! Good luck next time :)");

                _giveGuessingGameWinnersBytesCommand.Give(winners.Where(w => w.Difference <= 20).ToList());

                twitchClient.SendMessage(streamerChannelName,
                    winners[0].Difference > 20
                        ? $"@{string.Join(", @", winners.Select(w => w.Username))} has won... nothing!" +
                          $" {string.Join(", ", winners.Select(w => $"{w.Username} guessed {w.Guess}%"))} " +
                          $"You were {winners[0].Difference} away from the actual score. Do you even know {streamerChannelName}?"
                        : winners[0].Difference == 0
                            ? $"@{string.Join(", @", winners.Select(w => w.Username))} has won! You guessed {winners[0].Guess}. You were spot on! You've received {winners[0].BytesWon} bytes"
                            : $"@{string.Join(", @", winners.Select(w => w.Username))} has won! " +
                              $"{string.Join(", ", winners.Select(w => $"{w.Username} guessed {w.Guess}% "))}" +
                              $"You were {winners[0].Difference} away from the actual score. You've received {winners[0].BytesWon} bytes");

                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e,
                    $"Encountered an error when setting percentage and finishing guessing game. finalPercentage: {finalPercentage}");
                return false;
            }
        }

        public bool SubmitOrUpdateGuess(string username, decimal percentageGuess)
        {
            try
            {
                _submitOrUpdateGuessCommand.SubmitOrUpdate(username, percentageGuess);
                return true;
            }
            catch (Exception e)
            {
                _logger.LogInformation(e,
                    $"Encountered an error while trying to record a user's guess for the guessing game. username: {username}, percentageGuess: {percentageGuess}");
                return false;
            }
        }

        public bool IsGuessingGameInProgress()
        {
            try
            {
                var inProgress = _getGuessingGameStateQuery.InProgress();

                return inProgress;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error encountered when checking if there was a guessing game in progress.");
                return true;
            }
        }

        public bool SetGuessingGameState(bool state)
        {
            try
            {
                _setGuessingGameStateCommand.Set(state);
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error encountered when setting the GuessingGameState. State: {state}");
                return false;
            }
        }
    }
}