using System;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Playlist;
using CoreCodedChatbot.ApiContract.RequestModels.Playlist;
using CoreCodedChatbot.Database.Context;
using CoreCodedChatbot.Database.Context.Interfaces;

namespace CoreCodedChatbot.ApiApplication.Repositories.Playlist
{
    public class EditRequestRepository : IEditRequestRepository
    {
        private readonly IChatbotContextFactory _chatbotContextFactory;

        public EditRequestRepository(
            IChatbotContextFactory chatbotContextFactory
            )
        {
            _chatbotContextFactory = chatbotContextFactory;
        }

        public void Edit(int songRequestId, string requestText, string username, bool isMod)
        {
            using (var context = _chatbotContextFactory.Create())
            {
                var songRequest = context.SongRequests.Find(songRequestId);

                if (songRequest.RequestUsername != username && !isMod)
                    throw new UnauthorizedAccessException(
                        $"{username} attempted to edit a request which was not theirs: {songRequestId}");

                songRequest.RequestText = requestText;
                context.SaveChanges();
            }
        }
    }
}