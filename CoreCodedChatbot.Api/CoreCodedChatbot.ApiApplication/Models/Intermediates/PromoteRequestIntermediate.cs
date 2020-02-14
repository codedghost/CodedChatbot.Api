using CoreCodedChatbot.ApiContract.Enums.Playlist;

namespace CoreCodedChatbot.ApiApplication.Models.Intermediates
{
    public class PromoteRequestIntermediate
    {
        public PromoteRequestResult PromoteRequestResult { get; set; }
        public int PlaylistIndex { get; set; }
    }
}