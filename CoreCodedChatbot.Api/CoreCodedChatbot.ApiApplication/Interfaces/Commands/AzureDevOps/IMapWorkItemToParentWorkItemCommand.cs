using CoreCodedChatbot.ApiContract.ResponseModels.DevOps.ChildModels;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Commands.AzureDevOps;

public interface IMapWorkItemToParentWorkItemCommand
{
    DevOpsWorkItem Map(WorkItem parentWorkItem);
}