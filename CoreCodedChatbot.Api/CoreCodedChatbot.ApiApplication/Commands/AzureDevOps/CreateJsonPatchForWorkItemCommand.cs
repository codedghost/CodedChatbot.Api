using System.Linq;
using CoreCodedChatbot.ApiApplication.Extensions;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.AzureDevOps;
using CoreCodedChatbot.ApiContract.ResponseModels.DevOps.ChildModels;
using Microsoft.VisualStudio.Services.WebApi.Patch.Json;

namespace CoreCodedChatbot.ApiApplication.Commands.AzureDevOps
{
    public class CreateJsonPatchForWorkItemCommand : ICreateJsonPatchForWorkItemCommand
    {
        public CreateJsonPatchForWorkItemCommand() { }

        public JsonPatchDocument Create(string twitchUsername, DevOpsWorkItem workItem)
        {
            var jsonPatchDocument = new JsonPatchDocument();

            jsonPatchDocument
                .AddTitle($"{twitchUsername} - {workItem.Title}")
                .AddAcceptanceCriteria(workItem.AcceptanceCriteria);

            if (workItem.Tags != null && workItem.Tags.Any())
                jsonPatchDocument.AddTags(workItem.Tags);

            return jsonPatchDocument;
        }
    }
}