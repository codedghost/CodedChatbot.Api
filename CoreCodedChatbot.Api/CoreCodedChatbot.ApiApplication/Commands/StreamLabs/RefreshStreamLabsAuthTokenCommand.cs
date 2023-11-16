using System.Collections.Generic;
using System.Net.Http;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Settings;
using CoreCodedChatbot.ApiApplication.Models.Intermediates;
using CoreCodedChatbot.Secrets;
using Newtonsoft.Json;

namespace CoreCodedChatbot.ApiApplication.Commands.StreamLabs;

public class RefreshStreamLabsAuthTokenCommand
{
    private readonly ISecretService _secretService;
    private readonly ISetOrCreateSettingRepository _setOrCreateSettingRepository;
    private readonly IGetSettingRepository _getSettingRepository;

    public RefreshStreamLabsAuthTokenCommand(
        ISecretService secretService,
        ISetOrCreateSettingRepository setOrCreateSettingRepository,
        IGetSettingRepository getSettingRepository)
    {
        _secretService = secretService;
        _setOrCreateSettingRepository = setOrCreateSettingRepository;
        _getSettingRepository = getSettingRepository;
    }

    public async void Refresh()
    {
        var refreshToken = _getSettingRepository.Get<string>("StreamLabsRefreshToken");

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

        _setOrCreateSettingRepository.Set("StreamLabsAccessToken", tokenModel.Token);
        _setOrCreateSettingRepository.Set("StreamLabsRefreshToken", tokenModel.RefreshToken);
    }
}