using System;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Playlist;
using CoreCodedChatbot.ApiApplication.Models.Intermediates;
using CoreCodedChatbot.Database.Context.Interfaces;

namespace CoreCodedChatbot.ApiApplication.Repositories.Playlist;

public class GetSongRequestByIdRepository : IGetSongRequestByIdRepository
{
    private readonly IChatbotContextFactory _chatbotContextFactory;

    public GetSongRequestByIdRepository(
        IChatbotContextFactory chatbotContextFactory
    )
    {
        _chatbotContextFactory = chatbotContextFactory;
    }

    public SongRequestIntermediate GetRequest(int id)
    {
        using (var context = _chatbotContextFactory.Create())
        {
            var songRequest = context.SongRequests.Find(id);

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
}