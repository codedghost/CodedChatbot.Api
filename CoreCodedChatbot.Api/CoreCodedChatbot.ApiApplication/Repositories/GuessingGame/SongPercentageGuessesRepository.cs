using CoreCodedChatbot.Database.Context.Models;
using System.Collections.Generic;
using System.Linq;
using CoreCodedChatbot.ApiApplication.Repositories.Abstractions;
using CoreCodedChatbot.Database.Context.Interfaces;
using System.Threading.Tasks;

namespace CoreCodedChatbot.ApiApplication.Repositories.GuessingGame;

public class SongPercentageGuessesRepository : BaseRepository<SongPercentageGuess>
{
    public SongPercentageGuessesRepository(IChatbotContextFactory chatbotContextFactory) : base(chatbotContextFactory)
    {
    }

    public List<SongPercentageGuess> Get(int guessingGameRecordId)
    {
        var potentialWinnerModels = Context.SongPercentageGuesses
            .Where(g => g.SongGuessingRecord.SongGuessingRecordId == guessingGameRecordId).ToList();

        return potentialWinnerModels;
    }

    public async Task Submit(int gameId, string username, decimal percentageGuess)
    {
        var existingGuess = Context.SongPercentageGuesses.SingleOrDefault(g =>
            g.SongGuessingRecordId == gameId && g.Username == username);

        if (existingGuess == null)
        {
            existingGuess = new SongPercentageGuess
            {
                Guess = percentageGuess,
                SongGuessingRecordId = gameId,
                Username = username
            };

            await CreateAsync(existingGuess);
        }

        existingGuess.Guess = percentageGuess;

        await Context.SaveChangesAsync();
    }
}