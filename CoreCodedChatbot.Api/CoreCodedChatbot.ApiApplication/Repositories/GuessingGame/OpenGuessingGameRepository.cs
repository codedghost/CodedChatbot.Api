using System;
using System.Linq;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.GuessingGame;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;
using Microsoft.Extensions.Logging;

namespace CoreCodedChatbot.ApiApplication.Repositories.GuessingGame;

public class OpenGuessingGameRepository : IOpenGuessingGameRepository
{
    private readonly IChatbotContextFactory _chatbotContextFactory;
    private readonly ILogger<IOpenGuessingGameRepository> _logger;

    public OpenGuessingGameRepository(
        IChatbotContextFactory chatbotContextFactory,
        ILogger<IOpenGuessingGameRepository> logger
    )
    {
        _chatbotContextFactory = chatbotContextFactory;
        _logger = logger;
    }

    public bool Open(string songName)
    {
        try
        {
            using (var context = _chatbotContextFactory.Create())
            {
                // Unlikely, but ensure that other open games are closed
                var unclosedGames = context.SongGuessingRecords.Where(x => x.UsersCanGuess || x.IsInProgress);

                foreach (var unclosedGame in unclosedGames)
                {
                    unclosedGame.UsersCanGuess = false;
                    unclosedGame.IsInProgress = false;
                }

                var newGuessRecord = new SongGuessingRecord
                {
                    SongDetails = songName,
                    UsersCanGuess = true,
                    IsInProgress = true
                };

                context.SongGuessingRecords.Add(newGuessRecord);
                context.SaveChanges();

                return true;
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Encountered an error, returning false. SongName: {songName}");
            return false;
        }
    }
}