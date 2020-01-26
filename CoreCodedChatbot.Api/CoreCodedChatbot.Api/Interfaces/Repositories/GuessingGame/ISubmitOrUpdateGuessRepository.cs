namespace CoreCodedChatbot.Api.Interfaces.Repositories.GuessingGame
{
    public interface ISubmitOrUpdateGuessRepository
    {
        void Submit(int gameId, string username, decimal percentageGuess);
    }
}