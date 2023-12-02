using System.Linq;
using CoreCodedChatbot.ApiApplication.Repositories.Abstractions;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;

namespace CoreCodedChatbot.ApiApplication.Repositories.GuessingGame;

public class GetRunningGuessingGameIdRepository : BaseRepository<SongGuessingRecord>
{
    public GetRunningGuessingGameIdRepository(
        IChatbotContextFactory chatbotContextFactory
    ) : base(chatbotContextFactory)
    {
    }

    public int Get()
    {
        return Context.SongGuessingRecords?.SingleOrDefault(x => x.UsersCanGuess)?.SongGuessingRecordId ?? 0;

    }
}