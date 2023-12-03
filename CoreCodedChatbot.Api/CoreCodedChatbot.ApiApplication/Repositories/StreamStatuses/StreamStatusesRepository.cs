using CoreCodedChatbot.ApiApplication.Repositories.Abstractions;
using CoreCodedChatbot.ApiContract.RequestModels.StreamStatus;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace CoreCodedChatbot.ApiApplication.Repositories.StreamStatuses;

public class StreamStatusesRepository : BaseRepository<StreamStatus>
{
    private readonly ILogger _logger;

    public StreamStatusesRepository(
        IChatbotContextFactory chatbotContextFactory,
        ILogger logger) 
        : base(chatbotContextFactory)
    {
        _logger = logger;
    }

    public bool GetStreamStatus(string broadcasterUsername)
    {
        var status = Context.StreamStatuses.FirstOrDefault(s => s.BroadcasterUsername == broadcasterUsername);

        return status?.IsOnline ?? false;
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