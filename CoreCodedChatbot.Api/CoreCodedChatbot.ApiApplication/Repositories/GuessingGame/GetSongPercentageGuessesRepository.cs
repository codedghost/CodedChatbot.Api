using System.Collections.Generic;
using System.Linq;
using CoreCodedChatbot.ApiApplication.Repositories.Abstractions;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;

namespace CoreCodedChatbot.ApiApplication.Repositories.GuessingGame;

public class GetSongPercentageGuessesRepository : BaseRepository<SongPercentageGuess>
{
    public GetSongPercentageGuessesRepository(
        IChatbotContextFactory chatbotContextFactory
    ) : base(chatbotContextFactory)
    {
    }

    public List<SongPercentageGuess> Get(int guessingGameRecordId)
    {
        var potentialWinnerModels = Context.SongPercentageGuesses
            .Where(g => g.SongGuessingRecord.SongGuessingRecordId == guessingGameRecordId).ToList();

        return potentialWinnerModels;
    }
}