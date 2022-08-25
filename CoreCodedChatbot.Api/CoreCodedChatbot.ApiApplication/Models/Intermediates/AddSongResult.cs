using CoreCodedChatbot.ApiContract.Enums.Playlist;

namespace CoreCodedChatbot.ApiApplication.Models.Intermediates
{
    public class AddSongResult
    {
        public AddRequestResult AddRequestResult { get; set; }
        public int SongIndex { get; set; }
        public int SongRequestId { get; set; }
        public int MaximumRegularRequests { get; set; }
        public string FormattedSongText { get; set; }
    }
}