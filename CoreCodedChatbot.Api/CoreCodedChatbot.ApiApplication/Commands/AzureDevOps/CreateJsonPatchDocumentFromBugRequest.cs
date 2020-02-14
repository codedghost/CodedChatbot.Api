using CoreCodedChatbot.ApiApplication.Extensions;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.AzureDevOps;
using CoreCodedChatbot.ApiContract.ResponseModels.DevOps.ChildModels;
using Microsoft.VisualStudio.Services.WebApi.Patch.Json;

namespace CoreCodedChatbot.ApiApplication.Commands.AzureDevOps
{
    public class CreateJsonPatchDocumentFromBugRequestCommand : ICreateJsonPatchDocumentFromBugRequestCommand
    {
        public CreateJsonPatchDocumentFromBugRequestCommand(
            
            )
        {
            
        }

        public JsonPatchDocument Create(string twitchUsername, DevOpsBug bugInfo)
        {
            var jsonPatchDocument = new JsonPatchDocument();

            jsonPatchDocument
                .AddTitle($"{twitchUsername} - {bugInfo.Title}")
                .AddAcceptanceCriteria(bugInfo.AcceptanceCriteria)
                .AddReproSteps(bugInfo.ReproSteps)
                .AddSystemInfo(bugInfo.SystemInfo);

            return jsonPatchDocument;
        }
    }
}