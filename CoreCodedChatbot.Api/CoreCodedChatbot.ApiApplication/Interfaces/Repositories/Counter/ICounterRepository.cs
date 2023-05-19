using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Repositories.Abstractions;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Counter
{
    public interface ICounterRepository: IBaseRepository<Database.Context.Models.Counter>
    {
        Task<Database.Context.Models.Counter> UpdateCounter(string counterName);
    }
}