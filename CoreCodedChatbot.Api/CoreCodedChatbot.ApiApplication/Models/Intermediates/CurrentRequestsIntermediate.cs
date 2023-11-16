using System.Collections.Generic;

namespace CoreCodedChatbot.ApiApplication.Models.Intermediates;

public class CurrentRequestsIntermediate
{
    public List<BasicSongRequest> VipRequests { get; set; }
    public List<BasicSongRequest> RegularRequests { get; set; }
}