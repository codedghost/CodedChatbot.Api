using Newtonsoft.Json;

namespace CoreCodedChatbot.ApiApplication.Models.Intermediates
{
    public class StreamLabsDonationsIntermediate
    {
        [JsonProperty("data")]
        public StreamLabsDonationIntermediate[] StreamLabsDonations { get; set; }
    }
}