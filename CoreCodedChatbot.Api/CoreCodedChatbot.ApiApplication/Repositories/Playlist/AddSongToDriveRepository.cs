using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Playlist;
using CoreCodedChatbot.Database.Context.Interfaces;

namespace CoreCodedChatbot.ApiApplication.Repositories.Playlist;

public class AddSongToDriveRepository : IAddSongToDriveRepository
{
    private readonly IChatbotContextFactory _chatbotContextFactory;

    public AddSongToDriveRepository(
        IChatbotContextFactory chatbotContextFactory
    )
    {
        _chatbotContextFactory = chatbotContextFactory;
    }

    public bool AddSongToDrive(int songRequestId)
    {
        using (var context = _chatbotContextFactory.Create())
        {
            var songRequest = context.SongRequests.Find(songRequestId);

            if (songRequest == null || songRequest.Played || songRequest.InDrive)
                return false;

            songRequest.InDrive = true;

            context.SaveChanges();
        }

        return true;
    }
}