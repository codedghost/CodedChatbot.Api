using System.Collections.Generic;
using System.Linq;
using CoreCodedChatbot.Api.Interfaces.Repositories.Playlist;
using CoreCodedChatbot.Api.Models.Intermediates;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Library.Extensions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CoreCodedChatbot.Api.Repositories.Playlist
{
    public class GetUsersRequestsRepository : IGetUsersRequestsRepository
    {
        private readonly IChatbotContextFactory _chatbotContextFactory;

        public GetUsersRequestsRepository(
            IChatbotContextFactory chatbotContextFactory
        )
        {
            _chatbotContextFactory = chatbotContextFactory;
        }

        public List<UsersRequestsIntermediate> GetUsersRequests(string username)
        {
            using (var context = _chatbotContextFactory.Create())
            {
                var userRequests = context.SongRequests
                    .Where(sr => !sr.Played)
                    .OrderRequests()
                    .Select((sr, index) =>
                        new UsersRequestsIntermediate
                        {
                            SongRequestsText = sr.RequestText,
                            PlaylistPosition = index + 1,
                            IsVip = sr.VipRequestTime != null || sr.SuperVipRequestTime != null
                        });

                return userRequests.ToList();
            }
        }
    }
}