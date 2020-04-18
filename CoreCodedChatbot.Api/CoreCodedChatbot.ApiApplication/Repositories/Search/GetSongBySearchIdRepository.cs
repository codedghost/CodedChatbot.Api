using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Search;
using CoreCodedChatbot.ApiApplication.Models.Intermediates;
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

        public SongSearchIntermediate Get(int songId)
        {
            using (var context = _chatbotContextFactory.Create())
            {
                var song = context.Songs.Find(songId);

                return new SongSearchIntermediate
                {
                    SongId = song.SongId,
                    SongName = song.SongName,
                    SongArtist = song.SongArtist,
                    DownloadUrl = song.DownloadUrl
                };
            }
        }
    }
}