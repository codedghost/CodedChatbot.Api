using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Bytes;

public interface IGiveViewershipBytesRepository
{
    Task Give(List<string> usernames);
}