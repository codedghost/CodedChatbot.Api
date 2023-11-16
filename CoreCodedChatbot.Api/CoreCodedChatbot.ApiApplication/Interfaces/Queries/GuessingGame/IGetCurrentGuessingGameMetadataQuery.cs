using CoreCodedChatbot.ApiApplication.Models.Intermediates;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Queries.GuessingGame;

public interface IGetCurrentGuessingGameMetadataQuery
{
    FinishedGuessingGameMetadata Get();
}