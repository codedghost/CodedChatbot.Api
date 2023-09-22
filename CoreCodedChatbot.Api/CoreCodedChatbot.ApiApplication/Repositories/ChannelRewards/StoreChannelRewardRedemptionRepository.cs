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

        public void Store(Guid channelRewardsRedemptionId, Guid channelRewardId, string redeemedBy, bool processed)
        {
            using (var context = _chatbotContextFactory.Create())
            {
                var redemption = context.ChannelRewardRedemptions.Find(channelRewardsRedemptionId);

                if (redemption == null)
                {
                    redemption = new ChannelRewardRedemption
                    {
                        ChannelRewardId = channelRewardId,
                        Username = redeemedBy,
                        RedeemedAt = DateTime.Now
                    };

                    context.ChannelRewardRedemptions.Add(redemption);
                };

                redemption.Processed = processed;

                context.SaveChanges();
            }
        }
    }
}