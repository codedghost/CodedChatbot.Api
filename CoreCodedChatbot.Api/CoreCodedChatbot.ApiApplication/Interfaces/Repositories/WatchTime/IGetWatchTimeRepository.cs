using System;
using System.Threading.Tasks;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Repositories.WatchTime;

public interface IGetWatchTimeRepository
{
    Task<TimeSpan> Get(string username);
}