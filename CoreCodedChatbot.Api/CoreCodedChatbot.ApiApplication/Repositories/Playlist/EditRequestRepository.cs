using System;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Playlist;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;

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

        public void Edit(int songRequestId, string requestText, string username, bool isMod, int songId)
        {
            using (var context = _chatbotContextFactory.Create())
            {
                var songRequest = context.SongRequests.Find(songRequestId);

                if (songRequest.RequestUsername != username && !isMod)
                    throw new UnauthorizedAccessException(
                        $"{username} attempted to edit a request which was not theirs: {songRequestId}");

                songRequest.RequestText = requestText;
                songRequest.SongId = songId != 0 ? songId : (int?)null;
                songRequest.InDrive = songId != 0;
                context.SaveChanges();
            }
        }
    }
}