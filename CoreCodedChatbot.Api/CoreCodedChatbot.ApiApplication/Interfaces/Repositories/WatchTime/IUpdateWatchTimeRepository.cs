using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Repositories.WatchTime
{
    public interface IUpdateWatchTimeRepository
    {
        Task Update(IEnumerable<string> chatters);
    }
}