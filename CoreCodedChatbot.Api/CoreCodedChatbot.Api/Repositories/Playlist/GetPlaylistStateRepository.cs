using System;
using System.Linq;
using CoreCodedChatbot.Api.Interfaces.Repositories.Playlist;
using CoreCodedChatbot.ApiContract.Enums.Playlist;
using CoreCodedChatbot.Database.Context.Interfaces;

namespace CoreCodedChatbot.Api.Repositories.Playlist
{
    public class GetPlaylistStateRepository : IGetPlaylistStateRepository
    {
        private readonly IChatbotContextFactory _chatbotContextFactory;

        public GetPlaylistStateRepository(
            IChatbotContextFactory chatbotContextFactory
        )
        {
            _chatbotContextFactory = chatbotContextFactory;
        }

        public PlaylistState GetPlaylistState()
        {
            using (var context = _chatbotContextFactory.Create())
            {
                var status = context.Settings.SingleOrDefault(s => s.SettingName == "PlaylistStatus");

                return status?.SettingValue == null
                    ? PlaylistState.VeryClosed
                    : Enum.Parse<PlaylistState>(status.SettingValue);
            }
        }
    }
}