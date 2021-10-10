using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Models.Intermediates;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Commands.Playlist
{
    public interface IPromoteRequestCommand
    {
        Task<PromoteRequestIntermediate> Promote(string username, bool useSuperVip, int songRequestId = 0);
    }
}