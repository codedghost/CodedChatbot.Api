using Newtonsoft.Json;

namespace CoreCodedChatbot.ApiApplication.Models.Intermediates
{
    public class ChattersIntermediate
    {
        [JsonProperty("broadcaster")]
        public string[] Broadcasters { get; set; }

        [JsonProperty("moderators")]
        public string[] Mods { get; set; }

        [JsonProperty("staff")]
        public string[] Staff { get; set; }

        [JsonProperty("admins")]
        public string[] Admins { get; set; }

        [JsonProperty("global_mods")]
        public string[] GlobalMods { get; set; }

        [JsonProperty("vips")]
        public string[] Vips { get; set; }

        [JsonProperty("viewers")]
        public string[] Viewers { get; set; }
    }
}