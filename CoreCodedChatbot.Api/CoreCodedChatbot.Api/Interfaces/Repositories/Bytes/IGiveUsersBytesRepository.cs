using System.Collections.Generic;
using CoreCodedChatbot.Api.Models.Intermediates;

namespace CoreCodedChatbot.Api.Interfaces.Repositories.Bytes
{
    public interface IGiveUsersBytesRepository
    {
        void GiveBytes(List<GiveBytesToUserModel> users);
    }
}