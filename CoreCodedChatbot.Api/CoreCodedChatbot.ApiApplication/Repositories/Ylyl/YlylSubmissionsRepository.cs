using System;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Repositories.Abstractions;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;

namespace CoreCodedChatbot.ApiApplication.Repositories.Ylyl;

public class YlylSubmissionsRepository : BaseRepository<YlylSubmission>
{
    public YlylSubmissionsRepository(IChatbotContextFactory chatbotContextFactory) : base(chatbotContextFactory)
    {
    }

    public async Task CreateSubmission(
        int currentSessionId,
        ulong channelId,
        ulong messageId,
        int totalImages,
        int totalVideos)
    {
        var newSubmission = new YlylSubmission
        {
            SessionId = currentSessionId,
            ChannelId = channelId,
            MessageId = messageId,
            TotalImages = totalImages,
            TotalVideos = totalVideos,
            SubmissionTime = DateTime.UtcNow
        };

        await CreateAndSaveAsync(newSubmission);
    }
}