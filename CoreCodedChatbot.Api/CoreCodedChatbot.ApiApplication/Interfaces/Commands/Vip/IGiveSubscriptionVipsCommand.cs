using System.Collections.Generic;
using CoreCodedChatbot.ApiContract.RequestModels.Vip.ChildModels;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Commands.Vip;

public interface IGiveSubscriptionVipsCommand
{
    void Give(List<UserSubDetail> userSubDetails);
}