using System;
using System.Collections.Generic;
using CoreCodedChatbot.ApiApplication.Models.Intermediates;
using CoreCodedChatbot.ApiApplication.Repositories.Abstractions;
using CoreCodedChatbot.Config;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;
using CoreCodedChatbot.Database.DbExtensions;

namespace CoreCodedChatbot.ApiApplication.Repositories.Bytes;

public class GiveUsersBytesRepository : BaseRepository<User>
{
    private readonly IConfigService _configService;

    public GiveUsersBytesRepository(
        IChatbotContextFactory chatbotContextFactory,
        IConfigService configService
    ) : base(chatbotContextFactory)
    {
        _configService = configService;
    }

    public void GiveBytes(List<GiveBytesToUserModel> users)
    {
        foreach (var winner in users)
        {
            // Find or add user
            var user = Context.GetOrCreateUser(winner.Username);

            user.TokenBytes += (int) Math.Round(winner.Bytes * _configService.Get<int>("BytesToVip"));
        }

        Context.SaveChanges();
    }
}