using System.Linq;
using AutoFixture.NUnit3;
using CoreCodedChatbot.ApiApplication.Commands.AzureDevOps;
using CoreCodedChatbot.ApiApplication.Extensions;
using CoreCodedChatbot.ApiContract.ResponseModels.DevOps.ChildModels;
using Microsoft.VisualStudio.Services.WebApi.Patch.Json;
using NUnit.Framework;

namespace CoreCodedChatbot.ApiTests.Commands.AzureDevOps
{
    [TestFixture]
    public class CreateJsonPatchDocumentFromBugRequestCommandTests
    {
        private CreateJsonPatchDocumentFromBugRequestCommand _subject;

        [SetUp]
        public void Setup()
        {
            _subject = new CreateJsonPatchDocumentFromBugRequestCommand();
        }

        [Test, AutoData]
        public void EnsureJsonPatchIsGenerated(string twitchUsername, string title, string state, string assignedTo,
            string acceptanceCriteria, string reproSteps, string systemInfo)
        {
            var bug = new DevOpsBug
            {
                Title = title,
                State = state,
                AssignedTo = assignedTo,
                AcceptanceCriteria = acceptanceCriteria,
                ReproSteps = reproSteps,
                SystemInfo = systemInfo
            };

            var jsonPatch = _subject.Create(twitchUsername, bug);

            Assert.IsTrue(
                (string) jsonPatch.SingleOrDefault(j => j.Path == $"/fields/{AzureDevOpsFields.Title}")?.Value ==
                $"{twitchUsername} - {bug.Title}");
            Assert.IsTrue(
                (string) jsonPatch.SingleOrDefault(j => j.Path == $"/fields/{AzureDevOpsFields.AcceptanceCriteria}")
                    ?.Value == bug.AcceptanceCriteria);
            Assert.IsTrue(
                (string)jsonPatch.SingleOrDefault(j => j.Path == $"/fields/{AzureDevOpsFields.ReproSteps}")
                    ?.Value == bug.ReproSteps);
            Assert.IsTrue(
                (string)jsonPatch.SingleOrDefault(j => j.Path == $"/fields/{AzureDevOpsFields.SystemInfo}")
                    ?.Value == bug.SystemInfo);
        }
    }
}