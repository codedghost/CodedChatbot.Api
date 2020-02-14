using System;
using System.Linq;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.GuessingGame;
using CoreCodedChatbot.Database.Context.Interfaces;

namespace CoreCodedChatbot.ApiApplication.Repositories.GuessingGame
{
    public class CompleteGuessingGameRepository : ICompleteGuessingGameRepository
    {
        private readonly IChatbotContextFactory _chatbotContextFactory;

        public CompleteGuessingGameRepository(
            IChatbotContextFactory chatbotContextFactory
        )
        {
            _chatbotContextFactory = chatbotContextFactory;
        }

        public void CompleteCurrentGuessingGame(decimal finalPercentage)
        {
            using (var context = _chatbotContextFactory.Create())
            {
                var currentGuessingGame = context.SongGuessingRecords.FirstOrDefault(sr => sr.IsInProgress);

                if (currentGuessingGame == null)
                    throw new NullReferenceException("No current guessing games available"); 

                currentGuessingGame.IsInProgress = false;
                currentGuessingGame.FinalPercentage = finalPercentage;

                context.SaveChanges();
            }
        }
    }
}