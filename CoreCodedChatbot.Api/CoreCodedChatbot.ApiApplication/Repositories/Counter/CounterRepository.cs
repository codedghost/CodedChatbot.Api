using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Counter;
using CoreCodedChatbot.ApiApplication.Repositories.Abstractions;
using CoreCodedChatbot.Database.Context.Interfaces;

namespace CoreCodedChatbot.ApiApplication.Repositories.Counter
{
    public class CounterRepository : BaseRepository<Database.Context.Models.Counter>, ICounterRepository
    {
        public CounterRepository(IChatbotContextFactory chatbotContextFactory) : base(chatbotContextFactory)
        {
        }

        public async Task<Database.Context.Models.Counter> UpdateCounter(string counterName)
        {
            var counter = await GetByIdOrNullAsync(counterName);
            if (counter != null)
            {
                counter.CounterValue++;
                await _context.SaveChangesAsync();
                return counter;
            }

            var newCounter = new Database.Context.Models.Counter
            {
                CounterName = counterName,
                CounterSuffix = "Oofs",
                CounterValue = 1
            };

            await CreateAsync(newCounter);

            return newCounter;
        }
    }
}