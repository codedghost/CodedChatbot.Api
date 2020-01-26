using System.Collections.Generic;
using System.Linq;
using CoreCodedChatbot.Api.Interfaces.Repositories.GuessingGame;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;

namespace CoreCodedChatbot.Api.Repositories.GuessingGame
{
    public class GetSongPercentageGuessesRepository : IGetSongPercentageGuessesRepository
    {
        private readonly IChatbotContextFactory _chatbotContextFactory;

        public GetSongPercentageGuessesRepository(
            IChatbotContextFactory chatbotContextFactory
            )
        {
            _chatbotContextFactory = chatbotContextFactory;
        }

        public List<SongPercentageGuess> Get(int guessingGameRecordId)
        {
            using (var context = _chatbotContextFactory.Create())
            {
                var potentialWinnerModels = context.SongPercentageGuesses
                    .Where(g => g.SongGuessingRecord.SongGuessingRecordId == guessingGameRecordId).ToList();

                return potentialWinnerModels;
            }
        }
    }
}