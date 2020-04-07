using Newtonsoft.Json;

namespace CoreCodedChatbot.ApiApplication.Models.Intermediates
{
    public class TmiChattersIntermediate
    {
        [JsonProperty("_links")]
        public object Links { get; set; }

        [JsonProperty("chatter_count")]
        public int ChatterCount { get; set; }

        [JsonProperty("chatters")]
        public ChattersIntermediate ChattersIntermediate { get; set; }
    }
}