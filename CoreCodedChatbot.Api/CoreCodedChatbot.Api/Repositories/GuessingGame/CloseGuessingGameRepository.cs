using System;
using System.Linq;
using CoreCodedChatbot.Api.Interfaces.Repositories.GuessingGame;
using CoreCodedChatbot.Database.Context.Interfaces;
using Microsoft.Extensions.Logging;

namespace CoreCodedChatbot.Api.Repositories.GuessingGame
{
    public class CloseGuessingGameRepository : ICloseGuessingGameRepository
    {
        private readonly IChatbotContextFactory _chatbotContextFactory;
        private readonly ILogger<ICloseGuessingGameRepository> _logger;

        public CloseGuessingGameRepository(
            IChatbotContextFactory chatbotContextFactory,
            ILogger<ICloseGuessingGameRepository> logger
        )
        {
            _chatbotContextFactory = chatbotContextFactory;
            _logger = logger;
        }

        public bool Close()
        {
            try
            {
                using (var context = _chatbotContextFactory.Create())
                {
                    var currentGuessingGameRecords = context.SongGuessingRecords.Where(x => x.UsersCanGuess);

                    if (!currentGuessingGameRecords.Any())
                    {
                        _logger.LogInformation("Looks like this game is already closed");
                        return false;
                    }

                    var currentGuessingGameRecord = currentGuessingGameRecords.FirstOrDefault();

                    if (currentGuessingGameRecord == null)
                    {
                        _logger.LogWarning("This really shouldn't happen, currentGuessingGameRecord is null");
                        return false;
                    }

                    currentGuessingGameRecord.UsersCanGuess = false;
                    context.SaveChanges();

                    return true;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Encountered an error while closing the guessing game, returning false");
                return false;
            }
        }
    }
}