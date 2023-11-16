using CoreCodedChatbot.ApiContract.ResponseModels.DevOps.ChildModels;
using Microsoft.VisualStudio.Services.WebApi.Patch.Json;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Commands.AzureDevOps;

public interface ICreateJsonPatchDocumentFromProductBacklogItemRequestCommand
{
    JsonPatchDocument Create(string twitchUsername, DevOpsProductBacklogItem pbiInfo);
}