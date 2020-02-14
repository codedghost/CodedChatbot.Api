using CoreCodedChatbot.ApiApplication.Interfaces.Queries.StreamStatus;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.StreamStatus;

namespace CoreCodedChatbot.ApiApplication.Queries.StreamStatus
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