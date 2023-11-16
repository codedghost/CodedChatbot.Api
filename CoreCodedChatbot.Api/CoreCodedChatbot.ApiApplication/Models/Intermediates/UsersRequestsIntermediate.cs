namespace CoreCodedChatbot.ApiApplication.Models.Intermediates;

public class UsersRequestsIntermediate
{
    public int SongRequestId { get; set; }
    public string SongRequestsText { get; set; }
    public int PlaylistPosition { get; set; }
    public bool IsVip { get; set; }
    public bool IsSuperVip { get; set; }
}