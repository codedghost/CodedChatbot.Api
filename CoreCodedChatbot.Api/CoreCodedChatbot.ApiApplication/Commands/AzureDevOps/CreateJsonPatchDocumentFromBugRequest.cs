using CoreCodedChatbot.ApiApplication.Extensions;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.AzureDevOps;
using CoreCodedChatbot.ApiContract.ResponseModels.DevOps.ChildModels;
using Microsoft.VisualStudio.Services.WebApi.Patch.Json;

namespace CoreCodedChatbot.ApiApplication.Commands.AzureDevOps
{
    public class CreateJsonPatchDocumentFromBugRequestCommand : ICreateJsonPatchDocumentFromBugRequestCommand
    {
        private readonly ICreateJsonPatchForWorkItemCommand _createJsonPatchForWorkItemCommand;

        public CreateJsonPatchDocumentFromBugRequestCommand(
            ICreateJsonPatchForWorkItemCommand createJsonPatchForWorkItemCommand
            )
        {
            _createJsonPatchForWorkItemCommand = createJsonPatchForWorkItemCommand;
        }

        public JsonPatchDocument Create(string twitchUsername, DevOpsBug bugInfo)
        {
            var jsonPatchDocument = _createJsonPatchForWorkItemCommand.Create(twitchUsername, bugInfo);

            jsonPatchDocument
                .AddReproSteps(bugInfo.ReproSteps)
                .AddSystemInfo(bugInfo.SystemInfo);

            return jsonPatchDocument;
        }
    }
}