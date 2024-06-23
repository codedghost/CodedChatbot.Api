using System;
using System.Linq;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Models.Intermediates;
using CoreCodedChatbot.ApiApplication.Repositories.Abstractions;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;
using Microsoft.EntityFrameworkCore;

namespace CoreCodedChatbot.ApiApplication.Repositories.Ylyl;

public class YlylSessionsRepository : BaseRepository<YlylSession>
{
    public YlylSessionsRepository(IChatbotContextFactory chatbotContextFactory) : base(chatbotContextFactory)
    {
    }

    public async Task<bool> OpenNewSessionAsync()
    {
        var allSessions = GetAll();

        if (allSessions.Any(s => s.IsActive))
        {
            return false;
        }

        var newSession = new YlylSession
        {
            IsActive = true,
            OpenedAt = DateTime.UtcNow
        };

        await CreateAndSaveAsync(newSession);
        return true;
    }

    public async Task CloseSessionsAsync()
    {
        var allSessions = GetAll()
            .Where(s => s.IsActive);

        foreach (var session in allSessions)
        {
            session.IsActive = false;
            session.ClosedAt = DateTime.UtcNow;
        }

        await Context.SaveChangesAsync();
    }

    public async Task<int?> GetCurrentSessionId()
    {
        var currentSession = await GetAll()
            .SingleOrDefaultAsync(y => y.IsActive);

        return currentSession?.SessionId;
    }

    public YlylSessionTotalSubmissions? GetCurrentSessionSubmissions()
    {
        var currentSession = GetAll()
            .Include(y => y.YlylSubmissions)
            .SingleOrDefault(y => y.IsActive);

        if (currentSession == null)
        {
            return null;
        }

        return new YlylSessionTotalSubmissions
        {
            TotalImages = currentSession.YlylSubmissions.Sum(ys => ys.TotalImages),
            TotalVideos = currentSession.YlylSubmissions.Sum(ys => ys.TotalVideos)
        };
    }

    public YlylSessionTotalSubmissions GetUsersCurrentSessionSubmissions(ulong channelId)
    {
        var currentSessions = GetAll()
            .Include(y => y.YlylSubmissions);

        var currentSession = currentSessions
            .SingleOrDefault(y => y.IsActive);

        if (currentSession == null)
        {
            currentSession = currentSessions.OrderByDescending(c => c.OpenedAt).First();
        }

        var usersSubmissions = currentSession.YlylSubmissions.Where(s => s.ChannelId == channelId);

        return new YlylSessionTotalSubmissions
        {
            TotalImages = usersSubmissions.Sum(y => y.TotalImages),
            TotalVideos = usersSubmissions.Sum(y => y.TotalVideos)
        };
    }
}