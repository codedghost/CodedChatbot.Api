using System;
using System.Linq;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Repositories.Abstractions;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;
using Microsoft.Extensions.Logging;

namespace CoreCodedChatbot.ApiApplication.Repositories.GuessingGame;

public class CloseGuessingGameRepository : BaseRepository<SongGuessingRecord>
{
    private readonly ILogger<CloseGuessingGameRepository> _logger;

    public CloseGuessingGameRepository(
        IChatbotContextFactory chatbotContextFactory,
        ILogger<CloseGuessingGameRepository> logger
    ) : base(chatbotContextFactory)
    {
        _logger = logger;
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