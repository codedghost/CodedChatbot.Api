using CoreCodedChatbot.Api.Interfaces.Queries;
using CoreCodedChatbot.Api.Interfaces.Repositories;

namespace CoreCodedChatbot.Api.Queries
{
    public class GetStreamStatusQuery : IGetStreamStatusQuery
    {
        private readonly IGetStreamStatusRepository _getStreamStatusRepository;

        public GetStreamStatusQuery(
            IGetStreamStatusRepository getStreamStatusRepository
            )
        {
            _getStreamStatusRepository = getStreamStatusRepository;
        }

        public bool Get(string broadcasterUsername)
        {
            var status = _getStreamStatusRepository.GetStreamStatus(broadcasterUsername);

            return status;
        }
    }
}