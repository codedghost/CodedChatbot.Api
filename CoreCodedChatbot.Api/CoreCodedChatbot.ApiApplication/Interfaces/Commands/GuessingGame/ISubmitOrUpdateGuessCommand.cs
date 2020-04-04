namespace CoreCodedChatbot.ApiApplication.Interfaces.Commands.GuessingGame
{
    public interface ISubmitOrUpdateGuessCommand
    {
        void SubmitOrUpdate(string username, decimal percentageGuess);
    }
}