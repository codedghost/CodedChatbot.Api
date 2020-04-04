using System;
using CoreCodedChatbot.Api.Interfaces.Repositories;

namespace CoreCodedChatbot.Api.Commands
{
    public class SaveSearchSynonymRequestCommand : ISaveSearchSynonymRequestCommand
    {
        private readonly ISaveSearchSynonymRequestRepository _saveSearchSynonymRequestRepository;

        public SaveSearchSynonymRequestCommand(
            ISaveSearchSynonymRequestRepository saveSearchSynonymRequestRepository
        )
        {
            _saveSearchSynonymRequestRepository = saveSearchSynonymRequestRepository;
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
                // TODO: Add logger
                return false;
            }
        }
    }
}