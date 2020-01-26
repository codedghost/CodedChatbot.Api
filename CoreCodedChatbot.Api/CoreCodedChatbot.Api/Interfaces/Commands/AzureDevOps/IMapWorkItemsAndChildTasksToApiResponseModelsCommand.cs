using System.Collections.Generic;
using CoreCodedChatbot.ApiContract.ResponseModels.DevOps.ChildModels;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;

namespace CoreCodedChatbot.Api.Interfaces.Commands.AzureDevOps
{
    public interface IMapWorkItemsAndChildTasksToApiResponseModelsCommand
    {
        DevOpsWorkItem Map(WorkItem parentWorkItem, List<WorkItem> childWorkItems);
    }
}