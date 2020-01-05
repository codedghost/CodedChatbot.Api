using Microsoft.VisualStudio.Services.WebApi.Patch;
using Microsoft.VisualStudio.Services.WebApi.Patch.Json;

namespace CoreCodedChatbot.Api.Extensions
{
    public static class JsonPatchExtensions
    {
        public static JsonPatchDocument AddTitle(this JsonPatchDocument jsonPatchDocument, string title)
        {
            jsonPatchDocument.AddStringField(AzureDevOpsFields.Title, title);
            return jsonPatchDocument;
        }

        public static JsonPatchDocument AddAcceptanceCriteria(this JsonPatchDocument jsonPatchDocument, string acceptanceCriteria)
        {
            jsonPatchDocument.AddStringField(AzureDevOpsFields.AcceptanceCriteria, acceptanceCriteria);
            return jsonPatchDocument;
        }

        public static JsonPatchDocument AddReproSteps(this JsonPatchDocument jsonPatchDocument, string reproSteps)
        {
            jsonPatchDocument.AddStringField(AzureDevOpsFields.ReproSteps, reproSteps);
            return jsonPatchDocument;
        }

        public static JsonPatchDocument AddSystemInfo(this JsonPatchDocument jsonPatchDocument, string systemInfo)
        {
            jsonPatchDocument.AddStringField(AzureDevOpsFields.SystemInfo, systemInfo);
            return jsonPatchDocument;
        }

        public static JsonPatchDocument AddDescription(this JsonPatchDocument jsonPatchDocument, string description)
        {
            jsonPatchDocument.AddStringField(AzureDevOpsFields.Description, description);
            return jsonPatchDocument;
        }

        private static void AddStringField(this JsonPatchDocument jsonPatchDocument, string fieldName, string value)
        {
            AddField(jsonPatchDocument, fieldName, value ?? string.Empty);
        }

        private static void AddIntField(this JsonPatchDocument jsonPatchDocument, string fieldName, int? value)
        {
            AddField(jsonPatchDocument, fieldName, value ?? 0);
        }

        private static void AddField(JsonPatchDocument jsonPatchDocument, string fieldName, object value)
        {
            jsonPatchDocument.Add(new JsonPatchOperation
            {
                Operation = Operation.Add,
                Path = $"/fields/{fieldName}",
                Value = value
            });
        }
    }
}