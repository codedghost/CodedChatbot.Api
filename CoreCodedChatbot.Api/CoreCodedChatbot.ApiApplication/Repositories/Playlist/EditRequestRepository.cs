using System;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Repositories.Abstractions;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;

namespace CoreCodedChatbot.ApiApplication.Repositories.Playlist;

public class EditRequestRepository : BaseRepository<SongRequest>
{
    public EditRequestRepository(
        IChatbotContextFactory chatbotContextFactory
    ) : base(chatbotContextFactory)
    {
    }

    public async Task Edit(int songRequestId, string requestText, string username, bool isMod, int songId)
    {
        var songRequest = await GetByIdOrNullAsync(songRequestId);

        if (songRequest == null || (songRequest.Username != username && !isMod))
            throw new UnauthorizedAccessException(
                $"{username} attempted to edit a request which was not theirs: {songRequestId}");

        songRequest.RequestText = requestText;
        songRequest.SongId = songId != 0 ? songId : (int?) null;
        songRequest.InDrive = songId != 0;
        await Context.SaveChangesAsync();
    }
}