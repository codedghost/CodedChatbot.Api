using System.Collections.Generic;
using CoreCodedChatbot.ApiApplication.Models.Intermediates;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Queries.GuessingGame;

public interface IGetPotentialWinnersQuery
{
    List<GuessingGameWinner> Get(int guessingGameRecordId, decimal finishedPercentage);
}