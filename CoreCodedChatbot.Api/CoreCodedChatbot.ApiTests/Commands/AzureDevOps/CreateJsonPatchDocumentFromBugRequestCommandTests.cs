using System;
using System.Linq;
using AutoFixture.NUnit3;
using CoreCodedChatbot.ApiApplication.Commands.AzureDevOps;
using CoreCodedChatbot.ApiApplication.Extensions;
using CoreCodedChatbot.ApiContract.ResponseModels.DevOps.ChildModels;
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

            Assert.IsTrue(string.Equals(
                (string) jsonPatch.SingleOrDefault(j => j.Path == $"/fields/{AzureDevOpsFields.Title}")?.Value,
                $"{twitchUsername} - {bug.Title}", StringComparison.InvariantCulture));
            Assert.IsTrue(string.Equals(
                (string) jsonPatch.SingleOrDefault(j => j.Path == $"/fields/{AzureDevOpsFields.AcceptanceCriteria}")
                    ?.Value, bug.AcceptanceCriteria, StringComparison.InvariantCulture));
            Assert.IsTrue(string.Equals(
                (string)jsonPatch.SingleOrDefault(j => j.Path == $"/fields/{AzureDevOpsFields.ReproSteps}")
                    ?.Value, bug.ReproSteps, StringComparison.InvariantCulture));
            Assert.IsTrue(string.Equals(
                (string) jsonPatch.SingleOrDefault(j => j.Path == $"/fields/{AzureDevOpsFields.SystemInfo}")
                    ?.Value, bug.SystemInfo, StringComparison.InvariantCulture));
        }
    }
}