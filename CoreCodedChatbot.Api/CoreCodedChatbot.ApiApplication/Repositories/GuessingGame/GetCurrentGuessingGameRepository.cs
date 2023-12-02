using System.Linq;
using CoreCodedChatbot.ApiApplication.Repositories.Abstractions;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;

namespace CoreCodedChatbot.ApiApplication.Repositories.GuessingGame;

public class GetCurrentGuessingGameRepository : BaseRepository<SongGuessingRecord>
{
    public GetCurrentGuessingGameRepository(
        IChatbotContextFactory chatbotContextFactory
    ) : base(chatbotContextFactory)
    {
    }

    public SongGuessingRecord Get()
    {
        var songGuessingRecord = Context.SongGuessingRecords.SingleOrDefault(x => x.IsInProgress);

        return songGuessingRecord;
    }
}