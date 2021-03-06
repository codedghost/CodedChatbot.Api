﻿using CoreCodedChatbot.ApiContract.ResponseModels.DevOps.ChildModels;
using Microsoft.VisualStudio.Services.WebApi.Patch.Json;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Commands.AzureDevOps
{
    public interface ICreateJsonPatchDocumentFromBugRequestCommand
    {
        JsonPatchDocument Create(string twitchUsername, DevOpsBug bugInfo);
    }
}