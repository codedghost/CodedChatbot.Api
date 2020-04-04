namespace CoreCodedChatbot.ApiApplication.Models.Intermediates
{
    public class BasicSongRequest
    {
        public int SongRequestId { get; set; }
        public string SongRequestText { get; set; }
        public string Username { get; set; }
        public bool IsUserInChat { get; set; }
        public bool IsVip { get; set; }
        public bool IsSuperVip { get; set; }
        public bool IsEvenIndex { get; set; }
        public bool IsInDrive { get; set; }
    }
}