using System.Collections.Generic;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiContract.ResponseModels.Counters.ChildModels;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Services;

public interface ICounterService
{
    Task CreateCounter(string counterName, string? counterPreText, int? initialCountValue);
    Task<Counter> GetCounter(string counterName);
    Task<List<Counter>> GetCounters(int? page, int? pageSize, string? orderByColumnName, bool? desc, string? filterByColumn, object? filterValue);
    Task<Counter> UpdateCounter(string counterName);
}