using CoreCodedChatbot.ApiApplication.Models.Intermediates;
using CoreCodedChatbot.ApiApplication.Repositories.Abstractions;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;
using CoreCodedChatbot.Database.DbExtensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreCodedChatbot.Config;

namespace CoreCodedChatbot.ApiApplication.Repositories.Users;

public class UsersRepository : BaseRepository<User>
{
    private readonly IConfigService _configService;

    public UsersRepository(IChatbotContextFactory chatbotContextFactory, IConfigService configService) 
        : base(chatbotContextFactory)
    {
        _configService = configService;
    }


    #region Bytes

    public void GiveBytes(List<GiveBytesToUserModel> users)
    {
        foreach (var winner in users)
        {
            // Find or add user
            var user = Context.GetOrCreateUser(winner.Username);

            user.TokenBytes += (int)Math.Round(winner.Bytes * _configService.Get<int>("BytesToVip"));
        }

        Context.SaveChanges();
    }

    #endregion

    #region ClientIds

    public async Task<List<string>> GetClientIds(string username, string hubType)
    {
        var user = await GetByIdOrNullAsync(username);

        if (user == null) return new List<string>();

        var clientIds = user.GetClientIdsDictionary();

        return clientIds.ContainsKey(hubType) ? clientIds[hubType] : new List<string>();
    }

    public async Task RemoveClientId(string hubType, string clientId)
    {
        Context.Users.RemoveClientId(hubType, clientId);

        await Context.SaveChangesAsync();
    }

    public async Task StoreClientId(string hubType, string username, string clientId)
    {
        var user = Context.GetOrCreateUser(username);

        var clientIds = user.GetClientIdsDictionary();

        if (!clientIds.ContainsKey(hubType))
        {
            clientIds.Add(hubType, new List<string>());
        }

        clientIds[hubType].Add(clientId);

        user.UpdateClientIdsDictionary(clientIds);

        await Context.SaveChangesAsync();
    }

    #endregion

    #region Moderation

    public async Task TransferUser(string moderatorUsername, string oldUsername, string newUsername)
    {
        // All users should exist int the db at this point
        Context.TransferUser(moderatorUsername, oldUsername, newUsername);

        await Context.SaveChangesAsync();
    }

    #endregion

    #region WatchTime

    public async Task<TimeSpan> GetWatchTime(string username)
    {
        var user = await GetByIdOrNullAsync(username);

        return user == null ? TimeSpan.Zero : TimeSpan.FromMinutes(user.WatchTime);
    }

    public async Task UpdateWatchTime(IEnumerable<string> chatters)
    {
        foreach (var username in chatters)
        {
            var user = Context.GetOrCreateUser(username);

            user.WatchTime++;
            user.TimeLastInChat = DateTime.UtcNow;
        }

        await Context.SaveChangesAsync();
    }

    #endregion
}