using System.Collections.Generic;
using CoreCodedChatbot.ApiContract.ResponseModels.DevOps.ChildModels;
using Microsoft.VisualStudio.Services.WebApi.Patch.Json;

namespace CoreCodedChatbot.Api.Commands
{
    public interface ICreateJsonPatchDocumentFromProductBacklogItemRequestCommand
    {
        JsonPatchDocument Create(string twitchUsername, DevOpsProductBacklogItem pbiInfo, List<string> tags);
    }
}