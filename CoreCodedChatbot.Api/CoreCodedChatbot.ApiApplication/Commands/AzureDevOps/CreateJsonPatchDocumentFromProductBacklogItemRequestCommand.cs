using System.Collections.Generic;
using CoreCodedChatbot.ApiApplication.Extensions;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.AzureDevOps;
using CoreCodedChatbot.ApiContract.ResponseModels.DevOps.ChildModels;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.VisualStudio.Services.WebApi.Patch.Json;

namespace CoreCodedChatbot.ApiApplication.Commands.AzureDevOps
{
    public class CreateJsonPatchDocumentFromProductBacklogItemRequestCommand : ICreateJsonPatchDocumentFromProductBacklogItemRequestCommand
    {
        private readonly ICreateJsonPatchForWorkItemCommand _createJsonPatchForWorkItemCommand;

        public CreateJsonPatchDocumentFromProductBacklogItemRequestCommand(
            ICreateJsonPatchForWorkItemCommand createJsonPatchForWorkItemCommand
            )
        {
            _createJsonPatchForWorkItemCommand = createJsonPatchForWorkItemCommand;
        }

        public JsonPatchDocument Create(string twitchUsername, DevOpsProductBacklogItem pbiInfo)
        {
            var jsonPatchDocument = _createJsonPatchForWorkItemCommand.Create(twitchUsername, pbiInfo);

            jsonPatchDocument
                .AddDescription(pbiInfo.Description);

            return jsonPatchDocument;
        }
    }
}