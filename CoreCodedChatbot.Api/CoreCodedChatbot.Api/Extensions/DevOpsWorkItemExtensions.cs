using System;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;

namespace CoreCodedChatbot.Api.Extensions
{
    public static class DevOpsWorkItemExtensions
    {
        public static string Title(this WorkItem workItem) => GetFieldValueAsString(workItem, "System.Title");
        public static string State(this WorkItem workItem) => GetFieldValueAsString(workItem, "System.State");
        public static string AssignedTo(this WorkItem workItem) => GetFieldValueAsString(workItem, "System.AssignedTo");
        public static string AcceptanceCriteria(this WorkItem workItem) => GetFieldValueAsString(workItem, "System.AcceptanceCriteria");
        public static string Description(this WorkItem workItem) => GetFieldValueAsString(workItem, "System.Description");
        public static string ReproSteps(this WorkItem workItem) => GetFieldValueAsString(workItem, "System.ReproSteps");
        public static string SystemInfo(this WorkItem workItem) => GetFieldValueAsString(workItem, "System.SystemInfo");
        public static int RemainingWork(this WorkItem workItem) => GetFieldValueAsInt(workItem, "Microsoft.VSTS.Scheduling.RemainingWork");

        public static string WorkItemType(this WorkItem workItem) => GetFieldValueAsString(workItem, "System.WorkItemType");

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
}