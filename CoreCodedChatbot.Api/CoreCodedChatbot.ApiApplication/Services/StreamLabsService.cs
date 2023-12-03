using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Interfaces.Services;
using CoreCodedChatbot.ApiApplication.Models.Intermediates;
using CoreCodedChatbot.ApiApplication.Repositories.Settings;
using CoreCodedChatbot.ApiApplication.Repositories.Users;
using CoreCodedChatbot.Config;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Secrets;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CoreCodedChatbot.ApiApplication.Services;

public class StreamLabsService : IBaseService, IStreamLabsService
{
    private readonly IChatbotContextFactory _chatbotContextFactory;
    private readonly IConfigService _configService;
    private readonly ISecretService _secretService;
    private readonly ILogger<IStreamLabsService> _logger;
    private Timer CheckLatestDonationsTimer { get; set; }

    private const int MaxRetries = 2;

    public StreamLabsService(
        IChatbotContextFactory chatbotContextFactory,
        IConfigService configService,
        ISecretService secretService,
        ILogger<IStreamLabsService> logger
    )
    {
        _chatbotContextFactory = chatbotContextFactory;
        _configService = configService;
        _secretService = secretService;
        _logger = logger;
    }

    public void Initialise()
    {
        CheckLatestDonationsTimer = new Timer(e =>
            {
                CheckLatestDonations().Wait();
            },
            null, 
            TimeSpan.Zero,
            TimeSpan.FromMinutes(5));
    }

    private async Task CheckLatestDonations()
    {
        try
        {
            var attempts = 0;

            List<StreamLabsDonationIntermediate> donations = null;

            while (donations == null)
            {
                string accessToken;
                using (var repo = new SettingsRepository(_chatbotContextFactory))
                {
                    accessToken = repo.Get<string>("StreamLabsAccessToken");
                }

                if (string.IsNullOrWhiteSpace(accessToken))
                {
                    attempts++;

                    if (attempts >= MaxRetries) break;

                    continue;
                }

                string lastDonationId;
                using (var repo = new SettingsRepository(_chatbotContextFactory))
                {
                    lastDonationId = repo.Get<string>("LatestDonationId");
                }

                if (string.IsNullOrWhiteSpace(lastDonationId)) lastDonationId = string.Empty;

                var client = new HttpClient();

                var donationsResponse =
                    await client.GetAsync(
                        $"https://streamlabs.com/api/v1.0/donations?access_token={accessToken}&after={lastDonationId}&currency={_configService.Get<string>("DonationCurrency")}&limit=100");

                if (!donationsResponse.IsSuccessStatusCode)
                {
                    attempts++;

                    if (attempts >= MaxRetries) break;

                    continue;
                }

                var donationsJsonString = await donationsResponse.Content.ReadAsStringAsync();
                var donationsDeserialized = JsonConvert.DeserializeObject<StreamLabsDonationsIntermediate>(donationsJsonString);

                donations = donationsDeserialized.StreamLabsDonations.ToList();

                attempts++;

                if (attempts >= MaxRetries) break;
            }

            if (donations == null || !donations.Any()) return;

            await SaveStreamLabsDonations(donations);

            using (var repo = new UsersRepository(_chatbotContextFactory, _configService))
            {
                foreach (var user in donations.Select(d => d.Name.ToLower()).Distinct())
                {
                    var bitsToVip = _configService.Get<double>("BitsToVip");
                    var donationAmountToVip = _configService.Get<double>("DonationAmountToVip");

                    repo.UpdateDonationVips(user, bitsToVip, donationAmountToVip);
                }
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error when checking for latest StreamLabs donations");
        }
    }

    private async Task SaveStreamLabsDonations(List<StreamLabsDonationIntermediate> donations)
    {
        var latestDonationId = donations.OrderByDescending(d => d.CreateAt).FirstOrDefault()?.DonationId ?? 0;
        if (latestDonationId == 0) return;

        var groupedDonations = donations
            .OrderByDescending(d => d.CreateAt)
            .GroupBy(d => d.Name.ToLower())
            .Select(d => new StreamLabsDonation
            {
                Name = d.First().Name.ToLower(),
                Amount = (int)Math.Round(d.Sum(rec => rec.Amount) * 100)
            }).ToList();

        using (var repo = new UsersRepository(_chatbotContextFactory, _configService))
        {
            await repo.UpdateUsersDonations(groupedDonations);
        }

        using (var setOrCreateSettingRepository = new SettingsRepository(_chatbotContextFactory))
        {
            await setOrCreateSettingRepository.Set("LatestDonationId", latestDonationId.ToString());
        }
    }

    private async Task RefreshToken()
    {
        string refreshToken;
        using (var repo = new SettingsRepository(_chatbotContextFactory))
        {
            refreshToken = repo.Get<string>("StreamLabsRefreshToken");
        }

        if (string.IsNullOrWhiteSpace(refreshToken)) return;

        var vals = new Dictionary<string, string>
        {
            {"grant_type", "refresh_token"},
            {"client_id", _secretService.GetSecret<string>("StreamLabsClientId")},
            {"client_secret", _secretService.GetSecret<string>("StreamLabsClientSecret")},
            {"redirect_uri", "localhost"},
            {"refresh_token", refreshToken}
        };

        var encodedContent = new FormUrlEncodedContent(vals);
        var client = new HttpClient();
        var getTokenResponse = await client.PostAsync("https://streamlabs.com/api/v1.0/token", encodedContent);

        var tokenJsonString = await getTokenResponse.Content.ReadAsStringAsync();
        var tokenModel = JsonConvert.DeserializeObject<StreamLabsTokenIntermediate>(tokenJsonString);

        using (var repo = new SettingsRepository(_chatbotContextFactory))
        {
            await repo.Set("StreamLabsAccessToken", tokenModel.Token);
            await repo.Set("StreamLabsRefreshToken", tokenModel.RefreshToken);
        }
    }
}