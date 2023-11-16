using System;
using System.Linq;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.StreamStatus;
using CoreCodedChatbot.ApiContract.RequestModels.StreamStatus;
using CoreCodedChatbot.Database.Context.Interfaces;
using Microsoft.Extensions.Logging;

namespace CoreCodedChatbot.ApiApplication.Repositories.StreamStatus;

public class SaveStreamStatusRepository : ISaveStreamStatusRepository
{
    private readonly IChatbotContextFactory _chatbotContextFactory;
    private readonly ILogger<ISaveStreamStatusRepository> _logger;

    public SaveStreamStatusRepository(
        IChatbotContextFactory chatbotContextFactory,
        ILogger<ISaveStreamStatusRepository> logger
    )
    {
        _chatbotContextFactory = chatbotContextFactory;
        _logger = logger;
    }

    public bool Save(PutStreamStatusRequest request)
    {
        try
        {
            using (var context = _chatbotContextFactory.Create())
            {
                var currentStatus = context.StreamStatuses.FirstOrDefault(s =>
                    s.BroadcasterUsername == request.BroadcasterUsername);

                if (currentStatus == null)
                {
                    currentStatus = new Database.Context.Models.StreamStatus
                    {
                        BroadcasterUsername = request.BroadcasterUsername,
                        IsOnline = request.IsOnline
                    };

                    context.StreamStatuses.Add(currentStatus);
                    context.SaveChanges();
                    return true;
                }

                currentStatus.IsOnline = request.IsOnline;
                context.SaveChanges();
                return true;
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e,
                $"Exception caught when saving Stream Status. broadcasterUsername: {request.BroadcasterUsername}, isOnline: {request.IsOnline}");

            return false;
        }
    }
}