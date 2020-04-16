using CoreCodedChatbot.ApiContract.ResponseModels.Search.ChildModels;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Search
{
    public interface IGetSongBySearchIdRepository
    {
        BasicSongSearchResult Get(int songId);
    }
}