using System;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Models.Intermediates;
using CoreCodedChatbot.ApiApplication.Repositories.Abstractions;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;

namespace CoreCodedChatbot.ApiApplication.Repositories.Playlist;

public class GetSongRequestByIdRepository : BaseRepository<SongRequest>
{
    public GetSongRequestByIdRepository(
        IChatbotContextFactory chatbotContextFactory
    ) : base(chatbotContextFactory)
    {
    }

    public async Task<SongRequestIntermediate> GetRequest(int id)
    {
        var songRequest = await GetByIdOrNullAsync(id);

        if (songRequest == null) return null;

        var intermediate = new SongRequestIntermediate
        {
            SongRequestId = songRequest.SongRequestId,
            SongRequestText = songRequest.RequestText,
            SongRequestUsername = songRequest.Username,
            IsRecentRequest = songRequest.RequestTime.AddMinutes(5) >= DateTime.UtcNow,
            IsVip = songRequest.VipRequestTime != null,
            IsRecentVip = (songRequest.VipRequestTime ?? DateTime.MinValue).AddMinutes(5) >= DateTime.UtcNow,
            IsSuperVip = songRequest.SuperVipRequestTime != null,
            IsRecentSuperVip = (songRequest.SuperVipRequestTime ?? DateTime.MinValue).AddMinutes(5) >= DateTime.UtcNow,
            IsInDrive = songRequest.InDrive
        };

        return intermediate;
    }
}