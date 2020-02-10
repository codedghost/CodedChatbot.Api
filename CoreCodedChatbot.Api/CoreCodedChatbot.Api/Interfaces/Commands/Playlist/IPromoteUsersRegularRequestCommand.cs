using CoreCodedChatbot.Api.Models.Intermediates;
using CoreCodedChatbot.ApiContract.Enums.Playlist;

namespace CoreCodedChatbot.Api.Interfaces.Commands.Playlist
{
    public interface IPromoteUsersRegularRequestCommand
    {
        PromoteRequestIntermediate PromoteUsersRegularRequest(string username, int songRequestId = 0);
    }
}