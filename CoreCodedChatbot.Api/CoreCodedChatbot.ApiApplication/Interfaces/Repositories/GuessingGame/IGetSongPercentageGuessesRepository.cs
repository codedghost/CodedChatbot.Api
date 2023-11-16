using System.Collections.Generic;
using CoreCodedChatbot.Database.Context.Models;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Repositories.GuessingGame;

public interface IGetSongPercentageGuessesRepository
{
    List<SongPercentageGuess> Get(int guessingGameRecordId);
}