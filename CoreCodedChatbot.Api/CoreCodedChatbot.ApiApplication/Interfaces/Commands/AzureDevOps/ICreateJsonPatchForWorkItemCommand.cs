using CoreCodedChatbot.ApiContract.ResponseModels.DevOps.ChildModels;
using Microsoft.VisualStudio.Services.WebApi.Patch.Json;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Commands.AzureDevOps;

public interface ICreateJsonPatchForWorkItemCommand
{
    JsonPatchDocument Create(string twitchUsername, DevOpsWorkItem workItem);
}