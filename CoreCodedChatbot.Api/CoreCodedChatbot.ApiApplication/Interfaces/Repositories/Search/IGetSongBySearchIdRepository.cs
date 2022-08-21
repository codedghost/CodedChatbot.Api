using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Models.Intermediates;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Search
{
    public interface IGetSongBySearchIdRepository
    {
        Task<SongSearchIntermediate> Get(int songId);
    }
}