using CoreCodedChatbot.Database.Context.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Repositories.ChannelRewards;

public interface IGetChannelRewardRedemptionsRepository
{
    public Task<IEnumerable<ChannelRewardRedemption>> Get(bool includeNonCommandTypes);
}