using System.Collections.Generic;
using CoreCodedChatbot.Api.Intermediates;

namespace CoreCodedChatbot.Api.Interfaces.Queries.GuessingGame
{
    public interface IGetPotentialWinnersQuery
    {
        List<GuessingGameWinner> Get(int guessingGameRecordId, decimal finishedPercentage);
    }
}