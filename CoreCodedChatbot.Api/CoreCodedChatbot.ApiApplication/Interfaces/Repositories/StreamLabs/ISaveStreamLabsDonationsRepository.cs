using System.Collections.Generic;
using CoreCodedChatbot.ApiApplication.Models.Intermediates;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Repositories.StreamLabs
{
    public interface ISaveStreamLabsDonationsRepository
    {
        void Save(List<StreamLabsDonationIntermediate> donations);
    }
}