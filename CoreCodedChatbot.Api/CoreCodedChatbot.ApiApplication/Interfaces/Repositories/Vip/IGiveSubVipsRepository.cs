using System.Collections.Generic;
using CoreCodedChatbot.ApiContract.RequestModels.Vip.ChildModels;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Vip
{
    public interface IGiveSubVipsRepository
    {
        void Give(List<UserSubDetail> userSubDetails, int tier2ExtraVips, int tier3ExtraVips);
    }
}