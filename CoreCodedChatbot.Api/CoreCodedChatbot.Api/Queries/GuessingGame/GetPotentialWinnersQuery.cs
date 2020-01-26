using System;
using System.Collections.Generic;
using System.Linq;
using CoreCodedChatbot.Api.Interfaces.Queries.GuessingGame;
using CoreCodedChatbot.Api.Interfaces.Repositories.GuessingGame;
using CoreCodedChatbot.Api.Intermediates;

namespace CoreCodedChatbot.Api.Queries.GuessingGame
{
    public class GetPotentialWinnersQuery : IGetPotentialWinnersQuery
    {
        private readonly IGetSongPercentageGuessesRepository _getSongPercentageGuessesRepository;

        public GetPotentialWinnersQuery(
            IGetSongPercentageGuessesRepository getSongPercentageGuessesRepository
            )
        {
            _getSongPercentageGuessesRepository = getSongPercentageGuessesRepository;
        }

        public List<GuessingGameWinner> Get(int guessingGameRecordId, decimal finishedPercentage)
        {
            var guesses = _getSongPercentageGuessesRepository.Get(guessingGameRecordId);

            var potentialWinners = guesses
                .Select(pg =>
                    (Math.Floor(Math.Abs(finishedPercentage - pg.Guess) * 10) / 10,
                        pg)).ToList();

            return GuessingGameWinner.Create(potentialWinners);
        }
    }
}