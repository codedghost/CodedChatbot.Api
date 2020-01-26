namespace CoreCodedChatbot.Api.Interfaces.Repositories.GuessingGame
{
    public interface ICompleteGuessingGameRepository
    {
        void CompleteCurrentGuessingGame(decimal finalPercentage);
    }
}