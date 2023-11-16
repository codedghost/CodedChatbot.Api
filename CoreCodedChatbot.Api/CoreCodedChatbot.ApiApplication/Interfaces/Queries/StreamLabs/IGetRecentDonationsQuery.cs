using System.Collections.Generic;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Models.Intermediates;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Queries.StreamLabs;

public interface IGetRecentDonationsQuery
{
    Task<List<StreamLabsDonationIntermediate>> Get();
}