using CoreCodedChatbot.ApiContract.RequestModels.Playlist;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Commands.Playlist;

public interface IEditRequestCommand
{
    void Edit(EditWebRequestRequestModel model, int songId);
}