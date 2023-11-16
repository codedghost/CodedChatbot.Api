using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Interfaces.Queries.StreamLabs;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Settings;
using CoreCodedChatbot.ApiApplication.Models.Intermediates;
using CoreCodedChatbot.Config;
using Newtonsoft.Json;

namespace CoreCodedChatbot.ApiApplication.Queries.StreamLabs;

public class GetRecentDonationsQuery : IGetRecentDonationsQuery
{
    private readonly IConfigService _configService;
    private readonly IGetSettingRepository _getSettingRepository;

    public GetRecentDonationsQuery(
        IConfigService configService,
        IGetSettingRepository getSettingRepository)
    {
        _configService = configService;
        _getSettingRepository = getSettingRepository;
    }

    public async Task<List<StreamLabsDonationIntermediate>> Get()
    {
        var accessToken = _getSettingRepository.Get<string>("StreamLabsAccessToken");
        if (string.IsNullOrWhiteSpace(accessToken)) return null;

        var lastDonationId = _getSettingRepository.Get<string>("LatestDonationId");

        if (string.IsNullOrWhiteSpace(lastDonationId)) lastDonationId = string.Empty;

        var client = new HttpClient();

        var donationsResponse =
            await client.GetAsync(
                $"https://streamlabs.com/api/v1.0/donations?access_token={accessToken}&after={lastDonationId}&currency={_configService.Get<string>("DonationCurrency")}&limit=100");

        if (!donationsResponse.IsSuccessStatusCode) return null;

        var donationsJsonString = await donationsResponse.Content.ReadAsStringAsync();
        var donations = JsonConvert.DeserializeObject<StreamLabsDonationsIntermediate>(donationsJsonString);

        return donations.StreamLabsDonations.ToList();
    }
}