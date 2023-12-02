using System;
using System.Linq;
using CoreCodedChatbot.ApiApplication.Repositories.Abstractions;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;

namespace CoreCodedChatbot.ApiApplication.Repositories.GuessingGame;

public class CompleteGuessingGameRepository : BaseRepository<SongGuessingRecord>
{
    public CompleteGuessingGameRepository(
        IChatbotContextFactory chatbotContextFactory
    ) : base(chatbotContextFactory)
    {
    }

    public void CompleteCurrentGuessingGame(decimal finalPercentage)
    {
        var currentGuessingGame = Context.SongGuessingRecords.FirstOrDefault(sr => sr.IsInProgress);

        if (currentGuessingGame == null)
        {
            throw new NullReferenceException("No current guessing games available");
        }

        currentGuessingGame.IsInProgress = false;
        currentGuessingGame.FinalPercentage = finalPercentage;

        Context.SaveChanges();
    }
}