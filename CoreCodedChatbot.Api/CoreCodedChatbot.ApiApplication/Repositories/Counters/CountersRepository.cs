using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Repositories.Abstractions;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;

namespace CoreCodedChatbot.ApiApplication.Repositories.Counters;

public class CountersRepository : BaseRepository<Counter>
{
    public CountersRepository(IChatbotContextFactory chatbotContextFactory) : base(chatbotContextFactory)
    {
    }

    public async Task<Counter> UpdateCounter(string counterName)
    {
        var counter = await GetByIdOrNullAsync(counterName);

        if (counter == null)
        {
            counter = new Counter
            {
                CounterName = counterName,
                CounterSuffix = "Oofs",
                CounterValue = 0
            };
            await CreateAsync(counter);
        }

        counter.CounterValue++;
        await Context.SaveChangesAsync();

        return counter;
    }
}