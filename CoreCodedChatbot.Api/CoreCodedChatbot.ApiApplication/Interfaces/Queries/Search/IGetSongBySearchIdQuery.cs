using CoreCodedChatbot.ApiApplication.Models.Intermediates;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Queries.Search
{
    public interface IGetSongBySearchIdQuery
    {
        SongSearchIntermediate Get(int songId);
    }
}