using CoreCodedChatbot.Api.Intermediates;

namespace CoreCodedChatbot.Api.Interfaces.Queries.GuessingGame
{
    public interface IGetCurrentGuessingGameMetadataQuery
    {
        FinishedGuessingGameMetadata Get();
    }
}