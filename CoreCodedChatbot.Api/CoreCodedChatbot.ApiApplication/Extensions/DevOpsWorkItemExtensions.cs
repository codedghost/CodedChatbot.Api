using System;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;

namespace CoreCodedChatbot.ApiApplication.Extensions;

public static class DevOpsWorkItemExtensions
{
    public static string Title(this WorkItem workItem) => GetFieldValueAsString(workItem, AzureDevOpsFields.Title);
    public static string State(this WorkItem workItem) => GetFieldValueAsString(workItem, AzureDevOpsFields.State);
    public static string AssignedTo(this WorkItem workItem) => GetFieldValueAsString(workItem, AzureDevOpsFields.AssignedTo);
    public static string AcceptanceCriteria(this WorkItem workItem) => GetFieldValueAsString(workItem, AzureDevOpsFields.AcceptanceCriteria);
    public static string Description(this WorkItem workItem) => GetFieldValueAsString(workItem, AzureDevOpsFields.Description);
    public static string ReproSteps(this WorkItem workItem) => GetFieldValueAsString(workItem, AzureDevOpsFields.ReproSteps);
    public static string SystemInfo(this WorkItem workItem) => GetFieldValueAsString(workItem, AzureDevOpsFields.SystemInfo);
    public static int RemainingWork(this WorkItem workItem) => GetFieldValueAsInt(workItem, AzureDevOpsFields.RemainingWork);

    public static string WorkItemType(this WorkItem workItem) => GetFieldValueAsString(workItem, AzureDevOpsFields.WorkItemType);

    private static string GetFieldValueAsString(WorkItem workItem, string field)
    {
        try
        {
            return (string) workItem.Fields[field];
        }
        catch (Exception)
        {
            return string.Empty;
        }
    }

    private static int GetFieldValueAsInt(WorkItem workItem, string field)
    {
        try
        {
            var workItemField = workItem.Fields[field];
            return Convert.ToInt32(workItemField);
        }
        catch (Exception)
        {
            return 0;
        }
    }
}