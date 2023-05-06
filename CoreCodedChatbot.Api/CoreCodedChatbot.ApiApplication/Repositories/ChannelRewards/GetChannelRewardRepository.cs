using System;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.ChannelRewards;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace CoreCodedChatbot.ApiApplication.Repositories.ChannelRewards
{
    public class GetChannelRewardRepository : IGetChannelRewardRepository
    {
        private readonly IChatbotContextFactory _chatbotContextFactory;

        public GetChannelRewardRepository(IChatbotContextFactory chatbotContextFactory)
        {
            _chatbotContextFactory = chatbotContextFactory;
        }

        public ChannelReward GetById(Guid id)
        {
            using (var context = _chatbotContextFactory.Create())
            {
                var channelReward = context.ChannelRewards.Find(id);

                return channelReward;
            }
        }
    }
}