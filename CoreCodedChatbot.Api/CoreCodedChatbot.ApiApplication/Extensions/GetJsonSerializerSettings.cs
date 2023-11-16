using Newtonsoft.Json;

namespace CoreCodedChatbot.ApiApplication.Extensions;

public static class GetJsonSerializerSettings
{
    public static JsonSerializerSettings Get()
    {
        return new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto
        };
    }
}