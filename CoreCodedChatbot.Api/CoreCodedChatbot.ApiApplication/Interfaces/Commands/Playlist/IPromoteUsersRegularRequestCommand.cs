using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Models.Intermediates;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Commands.Playlist
{
    public interface IPromoteUsersRegularRequestCommand
    {
        Task<PromoteRequestIntermediate> PromoteUsersRegularRequest(string username, int songRequestId = 0);
    }
}