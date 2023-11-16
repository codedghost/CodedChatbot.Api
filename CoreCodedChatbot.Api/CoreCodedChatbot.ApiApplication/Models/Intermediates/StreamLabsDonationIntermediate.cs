using Newtonsoft.Json;

namespace CoreCodedChatbot.ApiApplication.Models.Intermediates;

public class StreamLabsDonationIntermediate
{
    [JsonProperty("donation_id")]
    public int DonationId { get; set; }

    [JsonProperty("create_at")]
    public string CreateAt { get; set; }

    [JsonProperty("currency")]
    public string Currency { get; set; }

    [JsonProperty("amount")]
    public double Amount { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("message")]
    public string Message { get; set; }

    [JsonProperty("email")]
    public string Email { get; set; }
}