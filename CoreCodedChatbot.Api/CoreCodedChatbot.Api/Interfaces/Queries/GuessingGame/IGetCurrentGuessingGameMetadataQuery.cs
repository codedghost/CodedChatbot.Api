using CoreCodedChatbot.Api.Models.Intermediates;

namespace CoreCodedChatbot.Api.Interfaces.Queries.GuessingGame
{
    public interface IGetCurrentGuessingGameMetadataQuery
    {
        FinishedGuessingGameMetadata Get();
    }
}