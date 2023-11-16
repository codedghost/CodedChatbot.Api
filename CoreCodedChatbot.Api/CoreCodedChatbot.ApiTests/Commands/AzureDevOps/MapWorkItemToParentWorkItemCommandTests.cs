using System.Collections.Generic;
using AutoFixture.NUnit3;
using CoreCodedChatbot.ApiApplication.Commands.AzureDevOps;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.AzureDevOps;
using CoreCodedChatbot.ApiApplication.Interfaces.Services;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Moq;
using NUnit.Framework;

namespace CoreCodedChatbot.ApiTests.Commands.AzureDevOps;

[TestFixture]
public class MapWorkItemToParentWorkItemCommandTests
{
    private Mock<IAzureDevOpsService> _azureDevOpsService;

    private Mock<IMapWorkItemsAndChildTasksToApiResponseModelsCommand>
        _mapWorkItemsAndChildTasksToApiResponseModelsCommand;

    private MapWorkItemToParentWorkItemCommand _subject;

    [SetUp]
    public void Setup()
    {
        _azureDevOpsService = new Mock<IAzureDevOpsService>();
        _mapWorkItemsAndChildTasksToApiResponseModelsCommand =
            new Mock<IMapWorkItemsAndChildTasksToApiResponseModelsCommand>();
    }

    [Test, AutoData]
    public void EnsureServicesAreCalled(WorkItem workItem, List<WorkItem> childWorkItems)
    {
        _azureDevOpsService.Setup(s => s.GetChildWorkItemsByPbi(workItem)).ReturnsAsync(childWorkItems);

        _subject = new MapWorkItemToParentWorkItemCommand(_azureDevOpsService.Object,
            _mapWorkItemsAndChildTasksToApiResponseModelsCommand.Object);

        var result = _subject.Map(workItem);

        _azureDevOpsService.Verify(s => s.GetChildWorkItemsByPbi(workItem), Times.Once);
        _mapWorkItemsAndChildTasksToApiResponseModelsCommand.Verify(s => s.Map(workItem, childWorkItems),
            Times.Once);
    }
}