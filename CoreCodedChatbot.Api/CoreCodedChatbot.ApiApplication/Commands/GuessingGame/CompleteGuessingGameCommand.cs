using CoreCodedChatbot.ApiApplication.Interfaces.Commands.GuessingGame;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.GuessingGame;

namespace CoreCodedChatbot.ApiApplication.Commands.GuessingGame;

public class CompleteGuessingGameCommand : ICompleteGuessingGameCommand
{
    private readonly ICompleteGuessingGameRepository _completeGuessingGameRepository;

    public CompleteGuessingGameCommand(
        ICompleteGuessingGameRepository completeGuessingGameRepository
    )
    {
        _completeGuessingGameRepository = completeGuessingGameRepository;
    }

    public void CompleteCurrentGuessingGame(decimal finalPercentage)
    {
        _completeGuessingGameRepository.CompleteCurrentGuessingGame(finalPercentage);
    }
}