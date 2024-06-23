using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Repositories.Abstractions;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;
using Microsoft.VisualStudio.Services.Common;

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
        ulong userId,
        int totalImages,
        int totalVideos)
    {
        var newSubmission = new YlylSubmission
        {
            SessionId = currentSessionId,
            ChannelId = channelId,
            UserId = userId,
            MessageId = messageId,
            TotalImages = totalImages,
            TotalVideos = totalVideos,
            SubmissionTime = DateTime.UtcNow
        };

        await CreateAndSaveAsync(newSubmission);
    }

    public async Task UpdateUsers(List<KeyValuePair<ulong, ulong>> submissionsToUpdate)
    {
        foreach (var submissionToUpdate in submissionsToUpdate)
        {
            var submissions = GetAll()
                .Where(s => s.ChannelId == submissionToUpdate.Key);

            submissions.ForEach((s) => s.UserId = submissionToUpdate.Value);

            await Context.SaveChangesAsync();
        }
    }
}