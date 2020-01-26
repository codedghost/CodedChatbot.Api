using System.Linq;
using CoreCodedChatbot.Api.Interfaces.Repositories.GuessingGame;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;

namespace CoreCodedChatbot.Api.Repositories.GuessingGame
{
    public class GetCurrentGuessingGameRepository : IGetCurrentGuessingGameRepository
    {
        private readonly IChatbotContextFactory _chatbotContextFactory;

        public GetCurrentGuessingGameRepository(
            IChatbotContextFactory chatbotContextFactory
            )
        {
            _chatbotContextFactory = chatbotContextFactory;
        }

        public SongGuessingRecord Get()
        {
            using (var context = _chatbotContextFactory.Create())
            {
                var songGuessingRecord = context.SongGuessingRecords.SingleOrDefault(x => x.IsInProgress);

                return songGuessingRecord;
            }
        }
    }
}