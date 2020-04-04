using System.Collections.Generic;
using CoreCodedChatbot.Api.Extensions;
using CoreCodedChatbot.ApiContract.ResponseModels.DevOps.ChildModels;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.VisualStudio.Services.WebApi.Patch.Json;

namespace CoreCodedChatbot.Api.Commands
{
    public class CreateJsonPatchDocumentFromProductBacklogItemRequestCommand : ICreateJsonPatchDocumentFromProductBacklogItemRequestCommand
    {
        public JsonPatchDocument Create(string twitchUsername, DevOpsProductBacklogItem pbiInfo, List<string> tags)
        {
            var jsonPatchDocument = new JsonPatchDocument();

            jsonPatchDocument
                .AddTitle($"{twitchUsername} - {pbiInfo.Title}")
                .AddDescription(pbiInfo.Description);

            if (tags.Any())
                jsonPatchDocument.AddTags(tags);

            return jsonPatchDocument;
        }
    }
}