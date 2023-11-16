using System.Collections.Generic;
using CoreCodedChatbot.ApiApplication.Models.Intermediates;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Bytes;

public interface IGiveUsersBytesRepository
{
    void GiveBytes(List<GiveBytesToUserModel> users);
}