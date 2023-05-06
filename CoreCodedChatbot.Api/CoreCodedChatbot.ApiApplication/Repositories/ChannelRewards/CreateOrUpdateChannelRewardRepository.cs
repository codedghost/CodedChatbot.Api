using System;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.ChannelRewards;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;

namespace CoreCodedChatbot.ApiApplication.Repositories.ChannelRewards
{
    public class CreateOrUpdateChannelRewardRepository : ICreateOrUpdateChannelRewardRepository
    {
        private readonly IChatbotContextFactory _chatbotContextFactory;

        public CreateOrUpdateChannelRewardRepository(IChatbotContextFactory chatbotContextFactory)
        {
            _chatbotContextFactory = chatbotContextFactory;
        }

        public void CreateOrUpdate(Guid rewardId, string rewardTitle, string rewardDescription)
        {
            using (var context = _chatbotContextFactory.Create())
            {
                var reward = context.ChannelRewards.Find(rewardId);

                if (reward == null)
                {
                    reward = new ChannelReward
                    {
                        ChannelRewardId = rewardId
                    };

                    context.ChannelRewards.Add(reward);
                }

                reward.RewardTitle = rewardTitle;
                reward.RewardDescription = rewardDescription;

                context.SaveChanges();
            }
        }
    }
}