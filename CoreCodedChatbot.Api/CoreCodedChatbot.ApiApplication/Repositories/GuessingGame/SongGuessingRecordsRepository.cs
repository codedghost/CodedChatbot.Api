using CoreCodedChatbot.ApiApplication.Repositories.Abstractions;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace CoreCodedChatbot.ApiApplication.Repositories.GuessingGame;

public class SongGuessingRecordRepository : BaseRepository<SongGuessingRecord>
{
    private readonly ILogger _logger;

    public SongGuessingRecordRepository(
        IChatbotContextFactory chatbotContextFactory,
        ILogger logger) 
        : base(chatbotContextFactory)
    {
        _logger = logger;
    }

    public async Task<bool> Open(string songName)
    {
        try
        {
            // Unlikely, but ensure that other open games are closed
            var unclosedGames = Context.SongGuessingRecords.Where(x => x.UsersCanGuess || x.IsInProgress);

            foreach (var unclosedGame in unclosedGames)
            {
                unclosedGame.UsersCanGuess = false;
                unclosedGame.IsInProgress = false;
            }

            var newGuessRecord = new SongGuessingRecord
            {
                SongDetails = songName,
                UsersCanGuess = true,
                IsInProgress = true
            };

            await CreateAndSaveAsync(newGuessRecord);

            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Encountered an error, returning false. SongName: {songName}");
            return false;
        }
    }

    public int GetRunningGuessingGame()
    {
        return Context.SongGuessingRecords?.SingleOrDefault(x => x.UsersCanGuess)?.SongGuessingRecordId ?? 0;
    }

    public SongGuessingRecord? GetCurrentGuessingGame()
    {
        return Context.SongGuessingRecords.SingleOrDefault(x => x.IsInProgress);
    }

    public async Task CompleteCurrentGuessingGame(decimal finalPercentage)
    {
        var currentGuessingGame = Context.SongGuessingRecords.FirstOrDefault(sr => sr.IsInProgress);

        if (currentGuessingGame == null)
        {
            throw new NullReferenceException("No current guessing games available");
        }

        currentGuessingGame.IsInProgress = false;
        currentGuessingGame.FinalPercentage = finalPercentage;

        await Context.SaveChangesAsync();
    }

    public async Task<bool> Close()
    {
        try
        {
            var currentGuessingGameRecords = Context.SongGuessingRecords.Where(x => x.UsersCanGuess);

            if (!currentGuessingGameRecords.Any())
            {
                _logger.LogInformation("Looks like this game is already closed");
                return false;
            }

            var currentGuessingGameRecord = currentGuessingGameRecords.First();

            currentGuessingGameRecord.UsersCanGuess = false;
            await Context.SaveChangesAsync();

            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Encountered an error while closing the guessing game, returning false");
            return false;
        }
    }
}