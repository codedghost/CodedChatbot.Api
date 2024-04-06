using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CoreCodedChatbot.ApiApplication.Interfaces.Services;
using CoreCodedChatbot.ApiApplication.Repositories.Counters;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;
using ApiCounter = CoreCodedChatbot.ApiContract.ResponseModels.Counters.ChildModels.Counter;

namespace CoreCodedChatbot.ApiApplication.Services;

public class CounterService : ICounterService, IBaseService
{
    private readonly IChatbotContextFactory _chatbotContextFactory;
    private readonly IMapper _mapper;

    public CounterService(
        IChatbotContextFactory chatbotContextFactory,
        IMapper mapper)
    {
        _chatbotContextFactory = chatbotContextFactory;
        _mapper = mapper;
    }

    public async Task CreateCounter(string counterName, string? counterPreText, int? initialCountValue)
    {
        var newEntity = new Counter
        {
            CounterName = counterName,
            CounterSuffix = counterPreText ?? "Oofs",
            CounterValue = initialCountValue ?? 0
        };

        using (var repo = new CountersRepository(_chatbotContextFactory))
        {
            await repo.CreateAndSaveAsync(newEntity);
        }
    }

    public async Task<ApiCounter> GetCounter(string counterName)
    {
        using (var repo = new CountersRepository(_chatbotContextFactory))
        {
            var counter = await repo.GetByIdAsync(counterName);

            return _mapper.Map<ApiCounter>(counter);
        }
    }

    public async Task<List<ApiCounter>> GetCounters(int? page, int? pageSize, string? orderByColumnName, bool? desc, string? filterByColumn, object? filterValue)
    {
        using (var repo = new CountersRepository(_chatbotContextFactory))
        {
            var counters = await repo.GetAllPagedAsync(page, pageSize, orderByColumnName, desc,
                filterByColumn, filterValue);

            return _mapper.Map<List<ApiCounter>>(counters);
        }
    }

    public async Task<ApiCounter> UpdateCounter(string counterName)
    {
        using (var repo = new CountersRepository(_chatbotContextFactory))
        {
            var counter = await repo.UpdateCounter(counterName);

            return _mapper.Map<ApiCounter>(counter);
        }
    }

    public async Task ResetCounter(string counterName)
    {
        using (var repo = new CountersRepository(_chatbotContextFactory))
        {
            await repo.ResetCounter(counterName);
        }
    }

    public async Task UpdateCounterSuffix(string counterName, string counterSuffix)
    {
        using (var repo = new CountersRepository(_chatbotContextFactory))
        {
            await repo.UpdateCounterSuffix(counterName, counterSuffix);
        }
    }
}