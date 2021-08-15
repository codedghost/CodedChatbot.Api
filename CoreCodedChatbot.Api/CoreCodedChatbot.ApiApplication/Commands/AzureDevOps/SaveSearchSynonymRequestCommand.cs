using System;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Search;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Search;
using Microsoft.Extensions.Logging;

namespace CoreCodedChatbot.ApiApplication.Commands.AzureDevOps
{
    public class SaveSearchSynonymRequestCommand : ISaveSearchSynonymRequestCommand
    {
        private readonly ISaveSearchSynonymRequestRepository _saveSearchSynonymRequestRepository;
        private readonly ILogger<ISaveSearchSynonymRequestCommand> _logger;

        public SaveSearchSynonymRequestCommand(
            ISaveSearchSynonymRequestRepository saveSearchSynonymRequestRepository,
            ILogger<ISaveSearchSynonymRequestCommand> logger
        )
        {
            _saveSearchSynonymRequestRepository = saveSearchSynonymRequestRepository;
            _logger = logger;
        }

        public bool Save(string synonymRequest, string username)
        {
            try
            {
                _saveSearchSynonymRequestRepository.Save(synonymRequest, username);

                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error when saving synonym");
                return false;
            }
        }
    }
}