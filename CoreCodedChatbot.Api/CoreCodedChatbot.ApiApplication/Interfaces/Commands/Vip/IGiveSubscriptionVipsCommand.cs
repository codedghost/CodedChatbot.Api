using System.Collections.Generic;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Commands.Vip
{
    public interface IGiveSubscriptionVipsCommand
    {
        void Give(List<string> usernames);
    }
}