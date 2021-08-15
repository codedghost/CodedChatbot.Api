using System.Collections.Generic;
using CoreCodedChatbot.ApiApplication.Interfaces.Queries.ClientId;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.ClientId;

namespace CoreCodedChatbot.ApiApplication.Queries.ClientId
{
    public class GetClientIdsQuery : IGetClientIdsQuery
    {
        private readonly IGetClientIdsRepository _getClientIdsRepository;

        public GetClientIdsQuery(IGetClientIdsRepository getClientIdsRepository)
        {
            _getClientIdsRepository = getClientIdsRepository;
        }

        public List<string> Get(string username, string hubType)
        {
            return _getClientIdsRepository.Get(username, hubType);
        }
    }
}