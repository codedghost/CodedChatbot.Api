namespace CoreCodedChatbot.ApiApplication.Interfaces.Repositories.GuessingGame;

public interface ICompleteGuessingGameRepository
{
    void CompleteCurrentGuessingGame(decimal finalPercentage);
}