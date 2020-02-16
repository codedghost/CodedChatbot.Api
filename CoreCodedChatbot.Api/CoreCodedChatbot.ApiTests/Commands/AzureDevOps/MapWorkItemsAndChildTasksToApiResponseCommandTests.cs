using System.Collections.Generic;
using System.Linq;
using AutoFixture.NUnit3;
using CoreCodedChatbot.ApiApplication.Commands.AzureDevOps;
using CoreCodedChatbot.ApiApplication.Extensions;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.AzureDevOps;
using CoreCodedChatbot.ApiContract.ResponseModels.DevOps.ChildModels;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Moq;
using NUnit.Framework;

namespace CoreCodedChatbot.ApiTests.Commands.AzureDevOps
{
    [TestFixture]
    public class MapWorkItemsAndChildTasksToApiResponseCommandTests
    {
        private Mock<IMapWorkItemToTaskCommand> _mapWorkItemToTaskCommand;

        private MapWorkItemsAndChildTasksToApiResponseModelsCommand _subject;

        [SetUp]
        public void Setup()
        {
            _mapWorkItemToTaskCommand = new Mock<IMapWorkItemToTaskCommand>();
            _mapWorkItemToTaskCommand.Setup(s => s.Map(It.IsAny<WorkItem>())).Returns(new DevOpsTask());

            _subject = new MapWorkItemsAndChildTasksToApiResponseModelsCommand(_mapWorkItemToTaskCommand.Object);
        }

        [Test, AutoData]
        public void GeneratesDevOpsProductBacklogItem(WorkItem workItem, List<WorkItem> childWorkItems)
        {
            workItem.Fields[AzureDevOpsFields.WorkItemType] = "Product Backlog Item";
            childWorkItems.ForEach(t => t.Fields[AzureDevOpsFields.WorkItemType] = "Task");

            var result = _subject.Map(workItem, childWorkItems);

            _mapWorkItemToTaskCommand.Verify(s => s.Map(It.IsAny<WorkItem>()), Times.Exactly(childWorkItems.Count));

            Assert.IsTrue(typeof(DevOpsProductBacklogItem) == result.GetType());

            var castResult = (DevOpsProductBacklogItem) result;
            Assert.AreEqual(workItem.Id, castResult.Id);
            Assert.AreEqual(workItem.Title(), castResult.Title);
            Assert.AreEqual(workItem.State(), castResult.State);
            Assert.AreEqual(workItem.AssignedTo(), castResult.AssignedTo);
            Assert.AreEqual(workItem.Description(), castResult.Description);
            Assert.AreEqual(childWorkItems.Count, castResult.Tasks.Count);
        }

        [Test, AutoData]
        public void GeneratesDevOpsBug(WorkItem workItem, List<WorkItem> childWorkItems)
        {
            workItem.Fields[AzureDevOpsFields.WorkItemType] = "Bug";
            childWorkItems.ForEach(t => t.Fields[AzureDevOpsFields.WorkItemType] = "Task");

            var result = _subject.Map(workItem, childWorkItems);

            _mapWorkItemToTaskCommand.Verify(s => s.Map(It.IsAny<WorkItem>()), Times.Exactly(childWorkItems.Count));

            Assert.IsTrue(typeof(DevOpsBug) == result.GetType());

            var castResult = (DevOpsBug) result;
            Assert.AreEqual(workItem.Id, castResult.Id);
            Assert.AreEqual(workItem.Title(), castResult.Title);
            Assert.AreEqual(workItem.State(), castResult.State);
            Assert.AreEqual(workItem.AssignedTo(), castResult.AssignedTo);
            Assert.AreEqual(workItem.AcceptanceCriteria(), castResult.AcceptanceCriteria);
            Assert.AreEqual(workItem.ReproSteps(), castResult.ReproSteps);
            Assert.AreEqual(workItem.SystemInfo(), castResult.SystemInfo);
            Assert.AreEqual(childWorkItems.Count, castResult.Tasks.Count);
        }

        [Test, AutoData]
        public void ReturnsNullWhen_InvalidWorkItems(WorkItem workItem, List<WorkItem> childWorkItems)
        {
            var result = _subject.Map(workItem, childWorkItems);

            Assert.IsNull(result);
        }

        [Test]
        public void ReturnsNullWhen_NullItems()
        {
            var result = _subject.Map(null, null);

            Assert.IsNull(result);
        }

        [Test, AutoData]
        public void ReturnsValidItemsWhen_NullChildItems(WorkItem workItem)
        {
            workItem.Fields[AzureDevOpsFields.WorkItemType] = "Bug";

            var result = _subject.Map(workItem, null);

            _mapWorkItemToTaskCommand.Verify(s => s.Map(It.IsAny<WorkItem>()), Times.Exactly(0));

            Assert.IsTrue(typeof(DevOpsBug) == result.GetType());

            var castResult = (DevOpsBug)result;
            Assert.AreEqual(workItem.Id, castResult.Id);
            Assert.AreEqual(workItem.Title(), castResult.Title);
            Assert.AreEqual(workItem.State(), castResult.State);
            Assert.AreEqual(workItem.AssignedTo(), castResult.AssignedTo);
            Assert.AreEqual(workItem.AcceptanceCriteria(), castResult.AcceptanceCriteria);
            Assert.AreEqual(workItem.ReproSteps(), castResult.ReproSteps);
            Assert.AreEqual(workItem.SystemInfo(), castResult.SystemInfo);
            Assert.AreEqual(0, castResult.Tasks.Count);
        }
    }
}