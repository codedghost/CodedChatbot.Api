using CoreCodedChatbot.Api.Interfaces.Commands.GuessingGame;
using CoreCodedChatbot.Api.Interfaces.Repositories.GuessingGame;

namespace CoreCodedChatbot.Api.Commands.GuessingGame
{
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
}