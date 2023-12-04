using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodedChatbot.TwitchFactories.Interfaces;
using CoreCodedChatbot.ApiApplication.Constants;
using CoreCodedChatbot.ApiApplication.Interfaces.Services;
using CoreCodedChatbot.ApiApplication.Models.Intermediates;
using CoreCodedChatbot.ApiApplication.Repositories.GuessingGame;
using CoreCodedChatbot.ApiApplication.Repositories.Settings;
using CoreCodedChatbot.ApiApplication.Repositories.Users;
using CoreCodedChatbot.Config;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;
using Microsoft.Extensions.Logging;

namespace CoreCodedChatbot.ApiApplication.Services;

public class GuessingGameService : IBaseService, IGuessingGameService
{
    private readonly IConfigService _configService;
    private readonly ILogger<IGuessingGameService> _logger;
    private readonly ITwitchClientFactory _twitchClientFactory;
    private readonly IChatbotContextFactory _chatbotContextFactory;

    public GuessingGameService(
        IConfigService configService,
        ILogger<IGuessingGameService> logger,
        ITwitchClientFactory twitchClientFactory,
        IChatbotContextFactory chatbotContextFactory
    )
    {
        _configService = configService;
        _logger = logger;
        _twitchClientFactory = twitchClientFactory;
        _chatbotContextFactory = chatbotContextFactory;
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

        using (var repo = new SongGuessingRecordsRepository(_chatbotContextFactory, _logger))
        {
            if (!await repo.Open(songName))
            {
                twitchClient.SendMessage(streamerChannelName, "I couldn't start the guessing game :S");
                return;
            }
        }

        await SetGuessingGameState(true);

        twitchClient.SendMessage(streamerChannelName,
            $"The guessing game has begun! You have {secondsForGuessingGame} seconds to !guess the accuracy that {streamerChannelName} will get on {songName}!");

        await Task.Delay(TimeSpan.FromSeconds(secondsForGuessingGame));

        using (var repo = new SongGuessingRecordsRepository(_chatbotContextFactory, _logger))
        {
            if (!await repo.Close())
            {
                twitchClient.SendMessage(streamerChannelName,
                    "I couldn't close the guessing game for some reason... SEND HALP");
            }
        }

        twitchClient.SendMessage(streamerChannelName, "The guessing game has now closed. Good luck everyone!");

        await SetGuessingGameState(false);
    }

    public async Task<bool> SetPercentageAndFinishGame(decimal finalPercentage)
    {
        try
        {
            var twitchClient = _twitchClientFactory.Get();

            SongGuessingRecord? songGuessingRecord;
            using (var repo = new SongGuessingRecordsRepository(_chatbotContextFactory, _logger))
            {
                songGuessingRecord = repo.GetCurrentGuessingGame();
            }

            var finishedGuessingGameMetadata = songGuessingRecord == null
                ? null
                : new FinishedGuessingGameMetadata
                {
                    GuessingGameRecordId = songGuessingRecord.SongGuessingRecordId,
                    GuessingGameFinishedPercentage = songGuessingRecord.FinalPercentage
                };

            using (var repo = new SongGuessingRecordsRepository(_chatbotContextFactory, _logger))
            {
                await repo.CompleteCurrentGuessingGame(finalPercentage);
            }

            var streamerChannelName = _configService.Get<string>("StreamerChannel");

            List<SongPercentageGuess> guesses;
            using (var repo = new SongPercentageGuessesRepository(_chatbotContextFactory))
            {
                guesses = repo.Get(finishedGuessingGameMetadata.GuessingGameRecordId);
            }

            var potentialWinners = guesses
                .Select(pg =>
                    (Math.Floor(Math.Abs(finalPercentage - pg.Guess) * 10) / 10,
                        pg)).ToList();

            var winners = GuessingGameWinner.Create(potentialWinners);

            // No-one guessed?
            if (!winners.Any())
                twitchClient.SendMessage(streamerChannelName, "Nobody guessed! Good luck next time :)");

            var bytesModel = winners.Select(w =>
                new GiveBytesToUserModel
                {
                    Username = w.Username,
                    Bytes = w.BytesWon
                }).ToList();

            using (var repo = new UsersRepository(_chatbotContextFactory, _configService, _logger))
            {
                repo.GiveBytes(bytesModel);
            }

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

    public async Task<bool> SubmitOrUpdateGuess(string username, decimal percentageGuess)
    {
        try
        {
            int currentGameId;
            using (var repo = new SongGuessingRecordsRepository(_chatbotContextFactory, _logger))
            {
                currentGameId = repo.GetRunningGuessingGame();
            }

            if (currentGameId == 0)
                throw new Exception("No game currently running");

            using (var repo = new SongPercentageGuessesRepository(_chatbotContextFactory))
            {
                await repo.Submit(currentGameId, username, percentageGuess);
            }

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
            using (var repo = new SettingsRepository(_chatbotContextFactory))
            {
                return repo.Get<bool>(SettingsKeys.GuessingGameStateSettingKey);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error encountered when checking if there was a guessing game in progress.");
            return true;
        }
    }

    public async Task<bool> SetGuessingGameState(bool state)
    {
        try
        {
            using (var repo = new SettingsRepository(_chatbotContextFactory))
            {
                await repo.Set(SettingsKeys.GuessingGameStateSettingKey, state.ToString().ToLower());
            }
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error encountered when setting the GuessingGameState. State: {state}");
            return false;
        }
    }
}