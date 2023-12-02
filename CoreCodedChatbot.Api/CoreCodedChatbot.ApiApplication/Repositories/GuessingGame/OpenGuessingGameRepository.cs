using System;
using System.Linq;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Repositories.Abstractions;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;
using Microsoft.Extensions.Logging;

namespace CoreCodedChatbot.ApiApplication.Repositories.GuessingGame;

public class OpenGuessingGameRepository : BaseRepository<SongGuessingRecord>
{
    private readonly ILogger<OpenGuessingGameRepository> _logger;

    public OpenGuessingGameRepository(
        IChatbotContextFactory chatbotContextFactory,
        ILogger<OpenGuessingGameRepository> logger
    ) : base(chatbotContextFactory)
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
}