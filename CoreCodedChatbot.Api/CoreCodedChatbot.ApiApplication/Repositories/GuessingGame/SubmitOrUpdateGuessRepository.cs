using System.Linq;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Repositories.Abstractions;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;

namespace CoreCodedChatbot.ApiApplication.Repositories.GuessingGame;

public class SubmitOrUpdateGuessRepository : BaseRepository<SongPercentageGuess>
{
    public SubmitOrUpdateGuessRepository(
        IChatbotContextFactory chatbotContextFactory
    ) : base(chatbotContextFactory)
    {
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