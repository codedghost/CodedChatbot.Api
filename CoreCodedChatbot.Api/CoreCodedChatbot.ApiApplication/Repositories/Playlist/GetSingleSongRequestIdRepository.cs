using System;
using System.Linq;
using CoreCodedChatbot.ApiApplication.Models.Enums;
using CoreCodedChatbot.ApiApplication.Repositories.Abstractions;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;

namespace CoreCodedChatbot.ApiApplication.Repositories.Playlist;

public class GetSingleSongRequestIdRepository : BaseRepository<SongRequest>
{
    public GetSingleSongRequestIdRepository(
        IChatbotContextFactory chatbotContextFactory
    ) : base(chatbotContextFactory)
    {
    }

    public int Get(string username, SongRequestType songRequestType)
    {
        var songRequests = Context.SongRequests.Where(sr => sr.Username == username && !sr.Played);

        var singleRequest = songRequestType switch
        {
            SongRequestType.Regular => 
                songRequests.SingleOrDefault(sr =>
                    sr.SuperVipRequestTime == null &&
                    sr.VipRequestTime == null),
            SongRequestType.Vip =>
                songRequests.SingleOrDefault(sr =>
                    sr.SuperVipRequestTime == null &&
                    sr.VipRequestTime != null),
            SongRequestType.SuperVip =>
                songRequests.SingleOrDefault(sr =>
                    sr.SuperVipRequestTime != null),
            SongRequestType.Any => throw new ArgumentOutOfRangeException(nameof(songRequestType), songRequestType, null),
            _ => throw new ArgumentOutOfRangeException(nameof(songRequestType), songRequestType, null)
        };

        return singleRequest?.SongRequestId ?? 0;
    }
}