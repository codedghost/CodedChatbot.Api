using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Search;
using CoreCodedChatbot.ApiContract.ResponseModels.Search.ChildModels;
using CoreCodedChatbot.Database.Context.Interfaces;

namespace CoreCodedChatbot.ApiApplication.Repositories.Search
{
    public class GetSongBySearchIdRepository : IGetSongBySearchIdRepository
    {
        private readonly IChatbotContextFactory _chatbotContextFactory;

        public GetSongBySearchIdRepository(
            IChatbotContextFactory chatbotContextFactory
            )
        {
            _chatbotContextFactory = chatbotContextFactory;
        }

        public BasicSongSearchResult Get(int songId)
        {
            using (var context = _chatbotContextFactory.Create())
            {
                var song = context.Songs.Find(songId);

                return new BasicSongSearchResult
                {
                    SongId = song.SongId,
                    SongName = song.SongName,
                    ArtistName = song.SongArtist,
                    DownloadUrl = song.DownloadUrl
                };
            }
        }
    }
}