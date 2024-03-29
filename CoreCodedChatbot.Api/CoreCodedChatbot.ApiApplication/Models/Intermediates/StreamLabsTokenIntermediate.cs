﻿using Newtonsoft.Json;

namespace CoreCodedChatbot.ApiApplication.Models.Intermediates;

public class StreamLabsTokenIntermediate
{
    [JsonProperty("access_token")]
    public string Token { get; set; }

    [JsonProperty("token_type")]
    public string Type { get; set; }

    [JsonProperty("expires_in")]
    public string ExpiresIn { get; set; }

    [JsonProperty("refresh_token")]
    public string RefreshToken { get; set; }
}