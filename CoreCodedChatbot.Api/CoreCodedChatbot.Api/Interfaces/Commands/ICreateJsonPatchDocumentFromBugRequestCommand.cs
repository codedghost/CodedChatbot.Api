using CoreCodedChatbot.ApiContract.ResponseModels.DevOps.ChildModels;
using Microsoft.VisualStudio.Services.WebApi.Patch.Json;

namespace CoreCodedChatbot.Api.Interfaces.Commands
{
    public interface ICreateJsonPatchDocumentFromBugRequestCommand
    {
        JsonPatchDocument Create(string twitchUsername, DevOpsBug bugInfo);
    }
}