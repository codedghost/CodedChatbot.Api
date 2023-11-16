using System.Linq;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.GuessingGame;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;

namespace CoreCodedChatbot.ApiApplication.Repositories.GuessingGame;

public class SubmitOrUpdateGuessRepository : ISubmitOrUpdateGuessRepository
{
    private readonly IChatbotContextFactory _chatbotContextFactory;

    public SubmitOrUpdateGuessRepository(
        IChatbotContextFactory chatbotContextFactory
    )
    {
        _chatbotContextFactory = chatbotContextFactory;
    }

    public void Submit(int gameId, string username, decimal percentageGuess)
    {
        using (var context = _chatbotContextFactory.Create())
        {
            var existingGuess = context.SongPercentageGuesses.SingleOrDefault(g =>
                g.SongGuessingRecordId == gameId && g.Username == username);


            if (existingGuess == null)
            {
                existingGuess = new SongPercentageGuess
                {
                    Guess = percentageGuess,
                    SongGuessingRecordId = gameId,
                    Username = username
                };
                context.SongPercentageGuesses.Add(existingGuess);
            }
            else
            {
                existingGuess.Guess = percentageGuess;
            }

            context.SaveChanges();
        }
    }
}