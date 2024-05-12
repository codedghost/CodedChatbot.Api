using System;
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
                CounterValue = 0,
                Archived = false
            };
            await CreateAsync(counter);
        }

        CheckCounterArchived(counter);

        counter.CounterValue++;
        await Context.SaveChangesAsync();

        return counter;
    }

    public async Task ResetCounter(string counterName)
    {
        var counter = await GetByIdAsync(counterName);

        CheckCounterArchived(counter);

        counter.CounterValue = 0;
        await Context.SaveChangesAsync();
    }

    public async Task UpdateCounterSuffix(string counterName, string newSuffix)
    {
        var counter = await GetByIdAsync(counterName);

        CheckCounterArchived(counter);

        counter.CounterSuffix = newSuffix;
        await Context.SaveChangesAsync();
    }

    public async Task<Counter> ArchiveCounter(string counterName)
    {
        var counter = await GetByIdAsync(counterName);

        counter.Archived = true;
        await Context.SaveChangesAsync();

        return counter;
    }

    private void CheckCounterArchived(Counter counter)
    {
        if (counter.Archived)
        {
            throw new Exception($"{counter.CounterName} could not be updated as it is archived");
        }
    }
}