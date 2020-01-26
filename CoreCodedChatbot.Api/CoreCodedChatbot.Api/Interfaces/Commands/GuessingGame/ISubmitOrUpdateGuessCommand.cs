namespace CoreCodedChatbot.Api.Interfaces.Commands.GuessingGame
{
    public interface ISubmitOrUpdateGuessCommand
    {
        void SubmitOrUpdate(string username, decimal percentageGuess);
    }
}