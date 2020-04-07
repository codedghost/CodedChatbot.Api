using System.Collections.Generic;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Bytes
{
    public interface IGiveViewershipBytesRepository
    {
        void Give(List<string> usernames);
    }
}