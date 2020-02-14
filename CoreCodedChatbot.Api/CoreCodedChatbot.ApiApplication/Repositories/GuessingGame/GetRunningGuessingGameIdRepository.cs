using System.Linq;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.GuessingGame;
using CoreCodedChatbot.Database.Context.Interfaces;

namespace CoreCodedChatbot.ApiApplication.Repositories.GuessingGame
{
    public class GetRunningGuessingGameIdRepository : IGetRunningGuessingGameIdRepository
    {
        private readonly IChatbotContextFactory _chatbotContextFactory;

        public GetRunningGuessingGameIdRepository(
            IChatbotContextFactory chatbotContextFactory
            )
        {
            _chatbotContextFactory = chatbotContextFactory;
        }

        public int Get()
        {
            using (var context = _chatbotContextFactory.Create())
            {
                return context.SongGuessingRecords?.SingleOrDefault(x => x.UsersCanGuess)?.SongGuessingRecordId ?? 0;
            }
        }
    }
}