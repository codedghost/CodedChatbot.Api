using AutoFixture.NUnit3;
using CoreCodedChatbot.ApiApplication.Commands.AzureDevOps;
using CoreCodedChatbot.ApiApplication.Extensions;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using NUnit.Framework;

namespace CoreCodedChatbot.ApiTests.Commands.AzureDevOps
{
    [TestFixture]
    public class MapWorkItemToTaskCommandTests
    {
        private MapWorkItemToTaskCommand _subject;

        [SetUp]
        public void SetUp()
        {
            _subject = new MapWorkItemToTaskCommand();
        }

        [Test]
        public void ReturnsNullWhen_NullWorkItemProvided()
        {
            var result = _subject.Map(null);

            Assert.IsNull(result);
        }

        [Test, AutoData]
        public void ReturnsNullWhen_NotATaskProvided(WorkItem workItem)
        {
            var result = _subject.Map(workItem);

            Assert.IsNull(result);
        }

        [Test, AutoData]
        public void ReturnsDevOpsTaskWhen_ValidTaskProvided(WorkItem workItem)
        {
            workItem.Fields[AzureDevOpsFields.WorkItemType] = "Task";

            var result = _subject.Map(workItem);

            Assert.AreEqual(workItem.Id, result.Id);
            Assert.AreEqual(workItem.Title(), result.Title);
            Assert.AreEqual(workItem.Description(), result.Description);
            Assert.AreEqual(workItem.State(), result.State);
            Assert.AreEqual(workItem.AssignedTo(), result.AssignedTo);
            Assert.AreEqual(workItem.RemainingWork(), result.RemainingWork);
        }
    }
}