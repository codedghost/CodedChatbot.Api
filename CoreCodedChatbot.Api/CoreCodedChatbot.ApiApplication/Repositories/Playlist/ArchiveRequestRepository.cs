using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Playlist;
using CoreCodedChatbot.Database.Context.Interfaces;

namespace CoreCodedChatbot.ApiApplication.Repositories.Playlist;

public class ArchiveRequestRepository : IArchiveRequestRepository
{
    private readonly IChatbotContextFactory _chatbotContextFactory;

    public ArchiveRequestRepository(
        IChatbotContextFactory chatbotContextFactory
    )
    {
        _chatbotContextFactory = chatbotContextFactory;
    }

    public string ArchiveRequest(int requestId)
    {
        using (var context = _chatbotContextFactory.Create())
        {
            var request = context.SongRequests.Find(requestId);

            if (request == null) return string.Empty;

            request.Played = true;

            context.SaveChanges();

            return request.Username;
        }
    }
}