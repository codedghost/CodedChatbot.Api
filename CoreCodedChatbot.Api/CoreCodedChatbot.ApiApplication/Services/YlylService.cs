using System;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Interfaces.Services;
using CoreCodedChatbot.ApiApplication.Repositories.Ylyl;
using CoreCodedChatbot.ApiContract.Enums.Ylyl;
using CoreCodedChatbot.ApiContract.RequestModels.Ylyl;
using CoreCodedChatbot.ApiContract.ResponseModels.Ylyl;
using CoreCodedChatbot.ApiContract.ResponseModels.Ylyl.ChildModels;
using CoreCodedChatbot.Database.Context.Interfaces;
using Microsoft.Extensions.Logging;

namespace CoreCodedChatbot.ApiApplication.Services
{
    public class YlylService : IBaseService, IYlylService
    {
        private readonly IChatbotContextFactory _chatbotContextFactory;
        private readonly ILogger<YlylService> _logger;

        public YlylService(
            IChatbotContextFactory chatbotContextFactory,
            ILogger<YlylService> logger)
        {
            _chatbotContextFactory = chatbotContextFactory;
            _logger = logger;
        }

        public async Task<YlylSessionResponse> ChangeSession(YlylSessionRequest request)
        {
            switch (request.YlylSessionOperation)
            {
                case YlylSessionOperation.Open:
                    return await OpenSession();
                case YlylSessionOperation.Close:
                    return await CloseSession();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public async Task SaveSubmission(YlylSubmissionRequest request)
        {
            int? currentSessionId;
            using (var repo = new YlylSessionsRepository(_chatbotContextFactory))
            {
                currentSessionId = await repo.GetCurrentSessionId();
            }

            if (currentSessionId == null)
            {
                throw new Exception("No current active session");
            }

            using (var repo = new YlylSubmissionsRepository(_chatbotContextFactory))
            {
                await repo.CreateSubmission(
                    currentSessionId.Value,
                    request.ChannelId,
                    request.MessageId,
                    request.TotalImages,
                    request.TotalVideos);
            }
        }

        public async Task<YlylGetSubmissionsResponse> GetSubmissions(YlylGetSubmissionsRequest request)
        {
            using (var repo = new YlylSessionsRepository(_chatbotContextFactory))
            {
                var submissions = repo.GetUsersCurrentSessionSubmissions(request.ChannelId);

                return new YlylGetSubmissionsResponse
                {
                    TotalImages = submissions.TotalImages,
                    TotalVideos = submissions.TotalVideos
                };
            }
        }

        private async Task<YlylSessionResponse> OpenSession()
        {
            try
            {
                using (var repo = new YlylSessionsRepository(_chatbotContextFactory))
                {
                    var result = await repo.OpenNewSessionAsync();

                    return new YlylSessionResponse
                    {
                        IsOperationSuccessful = result,
                        YlylSessionOperation = YlylSessionOperation.Open,
                        SubmissionsModel = new SubmissionsModel()
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex, "Error when opening new Ylyl session");

                return new YlylSessionResponse
                {
                    IsOperationSuccessful = false,
                    YlylSessionOperation = YlylSessionOperation.Open,
                    SubmissionsModel = new SubmissionsModel()
                };
            }
        }

        private async Task<YlylSessionResponse> CloseSession()
        {
            try
            {
                using (var repo = new YlylSessionsRepository(_chatbotContextFactory))
                {
                    var currentSessionEstimatedEntries = repo.GetCurrentSessionSubmissions();

                    await repo.CloseSessionsAsync();

                    return new YlylSessionResponse
                    {
                        IsOperationSuccessful = true,
                        YlylSessionOperation = YlylSessionOperation.Close,
                        SubmissionsModel = new SubmissionsModel
                        {
                            TotalImages = currentSessionEstimatedEntries?.TotalImages ?? 0,
                            TotalVideos = currentSessionEstimatedEntries?.TotalVideos ?? 0
                        }
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex, "Error when opening new Ylyl session");

                return new YlylSessionResponse
                {
                    IsOperationSuccessful = false,
                    YlylSessionOperation = YlylSessionOperation.Close,
                    SubmissionsModel = new SubmissionsModel()
                };
            }
        }
    }
}
