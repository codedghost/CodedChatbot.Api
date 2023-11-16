namespace CoreCodedChatbot.ApiApplication.Models.Intermediates;

public class SongRequestIntermediate
{
    public int SongRequestId { get; set; }
    public string SongRequestText { get; set; }
    public string SongRequestUsername { get; set; }
    public bool IsRecentRequest { get; set; }
    public bool IsVip { get; set; }
    public bool IsRecentVip { get; set; }
    public bool IsSuperVip { get; set; }
    public bool IsRecentSuperVip { get; set; }
    public bool IsInDrive { get; set; }
}