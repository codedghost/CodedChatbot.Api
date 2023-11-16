using System;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.WatchTime;
using CoreCodedChatbot.ApiApplication.Interfaces.Services;

namespace CoreCodedChatbot.ApiApplication.Services;

public class WatchTimeService : IWatchTimeService
{
    private readonly IGetWatchTimeRepository _getWatchTimeRepository;

    public WatchTimeService(IGetWatchTimeRepository getWatchTimeRepository)
    {
        _getWatchTimeRepository = getWatchTimeRepository;
    }

    public async Task<TimeSpan> GetWatchTime(string username)
    {
        return await _getWatchTimeRepository.Get(username);
    }
}