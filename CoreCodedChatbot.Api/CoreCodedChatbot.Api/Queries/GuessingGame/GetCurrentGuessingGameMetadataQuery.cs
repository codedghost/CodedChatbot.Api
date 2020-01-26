using CoreCodedChatbot.Api.Interfaces.Queries.GuessingGame;
using CoreCodedChatbot.Api.Interfaces.Repositories.GuessingGame;
using CoreCodedChatbot.Api.Intermediates;

namespace CoreCodedChatbot.Api.Queries.GuessingGame
{
    public class GetCurrentGuessingGameMetadataQuery : IGetCurrentGuessingGameMetadataQuery
    {
        private readonly IGetCurrentGuessingGameRepository _getCurrentGuessingGameRepository;

        public GetCurrentGuessingGameMetadataQuery(
            IGetCurrentGuessingGameRepository getCurrentGuessingGameRepository
        )
        {
            _getCurrentGuessingGameRepository = getCurrentGuessingGameRepository;
        }

        public FinishedGuessingGameMetadata Get()
        {
            var songGuessingRecord = _getCurrentGuessingGameRepository.Get();

            var finishedGuessingGameMetadata = songGuessingRecord == null
                ? null
                : new FinishedGuessingGameMetadata
                {
                    GuessingGameRecordId = songGuessingRecord.SongGuessingRecordId,
                    GuessingGameFinishedPercentage = songGuessingRecord.FinalPercentage
                };

            return finishedGuessingGameMetadata;
        }
    }
}