using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.ChannelRewards;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;
using Microsoft.EntityFrameworkCore;

namespace CoreCodedChatbot.ApiApplication.Repositories.ChannelRewards
{
    public class GetChannelRewardRedemptionsRepository : IGetChannelRewardRedemptionsRepository
    {
        private readonly IChatbotContextFactory _chatbotContextFactory;

        public GetChannelRewardRedemptionsRepository(IChatbotContextFactory chatbotContextFactory)
        {
            _chatbotContextFactory = chatbotContextFactory;
        }

        public async Task<IEnumerable<ChannelRewardRedemption>> Get(bool includeNonCommandTypes)
        {
            using (var context = _chatbotContextFactory.Create())
            {
                if (includeNonCommandTypes)
                {
                    return await context.ChannelRewardRedemptions
                        .Where(crr => !crr.Processed).ToListAsync();
                }

                var commandRewards = await context.ChannelRewards.Where(cr => cr.CommandType != 0).Select(cr => cr.ChannelRewardId).ToListAsync();

                return await context.ChannelRewardRedemptions
                    .Where(crr => !crr.Processed && commandRewards.Contains(crr.ChannelRewardId))
                    .ToListAsync();
            }
        }
    }
}