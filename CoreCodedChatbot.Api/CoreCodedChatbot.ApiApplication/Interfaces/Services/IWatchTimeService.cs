using System;
using System.Threading.Tasks;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Services;

public interface IWatchTimeService
{
    Task<TimeSpan> GetWatchTime(string username);
}