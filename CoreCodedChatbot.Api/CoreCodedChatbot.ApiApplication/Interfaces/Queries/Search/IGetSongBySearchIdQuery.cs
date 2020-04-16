using CoreCodedChatbot.ApiContract.ResponseModels.Search.ChildModels;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Queries.Search
{
    public interface IGetSongBySearchIdQuery
    {
        BasicSongSearchResult Get(int songId);
    }
}