using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Interfaces.Queries.Search;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Search;
using CoreCodedChatbot.ApiApplication.Models.Intermediates;

namespace CoreCodedChatbot.ApiApplication.Queries.Search;

public class GetSongBySearchIdQuery : IGetSongBySearchIdQuery
{
    private readonly IGetSongBySearchIdRepository _getSongBySearchIdRepository;

    public GetSongBySearchIdQuery(IGetSongBySearchIdRepository getSongBySearchIdRepository)
    {
        _getSongBySearchIdRepository = getSongBySearchIdRepository;
    }

    public async Task<SongSearchIntermediate> Get(int songId)
    {
        var songRequest = await _getSongBySearchIdRepository.Get(songId).ConfigureAwait(false);

        return songRequest;
    }
}