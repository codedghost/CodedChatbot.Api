using System;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.ChannelRewards;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;

namespace CoreCodedChatbot.ApiApplication.Repositories.ChannelRewards
{
    public class StoreChannelRewardRedemptionRepository : IStoreChannelRewardRedemptionRepository
    {
        private readonly IChatbotContextFactory _chatbotContextFactory;

        public StoreChannelRewardRedemptionRepository(IChatbotContextFactory chatbotContextFactory)
        {
            _chatbotContextFactory = chatbotContextFactory;
        }

        public void Store(Guid channelRewardId, string redeemedBy)
        {
            using (var context = _chatbotContextFactory.Create())
            {
                var redemption = new ChannelRewardRedemption
                {
                    ChannelRewardId = channelRewardId,
                    Username = redeemedBy,
                    RedeemedAt = DateTime.Now,
                    Processed = false
                };

                context.ChannelRewardRedemptions.Add(redemption);

                context.SaveChanges();
            }
        }
    }
}