using System;
using System.Linq;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Repositories.Abstractions;
using CoreCodedChatbot.ApiContract.RequestModels.StreamStatus;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;
using Microsoft.Extensions.Logging;

namespace CoreCodedChatbot.ApiApplication.Repositories.StreamStatuses;

public class SaveStreamStatusRepository : BaseRepository<StreamStatus>
{
    private readonly ILogger<SaveStreamStatusRepository> _logger;

    public SaveStreamStatusRepository(
        IChatbotContextFactory chatbotContextFactory,
        ILogger<SaveStreamStatusRepository> logger
    ) : base(chatbotContextFactory)
    {
        _logger = logger;
    }

    public async Task<bool> Save(PutStreamStatusRequest request)
    {
        try
        {
            var currentStatus = Context.StreamStatuses.FirstOrDefault(s =>
                s.BroadcasterUsername == request.BroadcasterUsername);

            if (currentStatus == null)
            {
                currentStatus = new StreamStatus
                {
                    BroadcasterUsername = request.BroadcasterUsername,
                    IsOnline = request.IsOnline
                };

                await CreateAndSaveAsync(currentStatus);
                return true;
            }

            currentStatus.IsOnline = request.IsOnline;

            await Context.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e,
                $"Exception caught when saving Stream Status. broadcasterUsername: {request.BroadcasterUsername}, isOnline: {request.IsOnline}");

            return false;
        }
    }
}