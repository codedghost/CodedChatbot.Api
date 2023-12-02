using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Counter;
using CoreCodedChatbot.ApiApplication.Interfaces.Services;
using CoreCodedChatbot.Database.Context.Models;
using ApiCounter = CoreCodedChatbot.ApiContract.ResponseModels.Counters.ChildModels.Counter;

namespace CoreCodedChatbot.ApiApplication.Services;

public class CounterService : IBaseService, ICounterService
{
    private readonly ICounterRepository _counterRepository;
    private readonly IMapper _mapper;

    public CounterService(
        ICounterRepository counterRepository,
        IMapper mapper)
    {
        _counterRepository = counterRepository;
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

        await _counterRepository.CreateAsync(newEntity);
    }

    public async Task<ApiCounter> GetCounter(string counterName)
    {
        var counter = await _counterRepository.GetByIdAsync(counterName);

        return _mapper.Map<ApiCounter>(counter);
    }

    public async Task<List<ApiCounter>> GetCounters(int? page, int? pageSize, string? orderByColumnName, bool? desc, string? filterByColumn, object? filterValue)
    {
        var counters = await _counterRepository.GetAllPagedAsync(page, pageSize, orderByColumnName, desc,
            filterByColumn, filterValue);

        return _mapper.Map<List<ApiCounter>>(counters);
    }

    public async Task<ApiCounter> UpdateCounter(string counterName)
    {
        var counter = await _counterRepository.UpdateCounter(counterName);

        return _mapper.Map<ApiCounter>(counter);
    }
}