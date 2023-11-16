using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Models.Intermediates;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Queries.Search;

public interface IGetSongBySearchIdQuery
{
    Task<SongSearchIntermediate> Get(int songId);
}