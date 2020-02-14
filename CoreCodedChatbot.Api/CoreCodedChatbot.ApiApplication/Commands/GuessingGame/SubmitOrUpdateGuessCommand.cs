using System;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.GuessingGame;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.GuessingGame;

namespace CoreCodedChatbot.ApiApplication.Commands.GuessingGame
{
    public class SubmitOrUpdateGuessCommand : ISubmitOrUpdateGuessCommand
    {
        private readonly ISubmitOrUpdateGuessRepository _submitOrUpdateGuessRepository;
        private readonly IGetRunningGuessingGameIdRepository _getRunningGuessingGameIdRepository;

        public SubmitOrUpdateGuessCommand(
            ISubmitOrUpdateGuessRepository submitOrUpdateGuessRepository,
            IGetRunningGuessingGameIdRepository getRunningGuessingGameIdRepository
            )
        {
            _submitOrUpdateGuessRepository = submitOrUpdateGuessRepository;
            _getRunningGuessingGameIdRepository = getRunningGuessingGameIdRepository;
        }

        public void SubmitOrUpdate(string username, decimal percentageGuess)
        {
            var currentGameId = _getRunningGuessingGameIdRepository.Get();

            if (currentGameId == 0)
                throw new Exception("No game currently running");

            _submitOrUpdateGuessRepository.Submit(currentGameId, username, percentageGuess);
        }
    }
}