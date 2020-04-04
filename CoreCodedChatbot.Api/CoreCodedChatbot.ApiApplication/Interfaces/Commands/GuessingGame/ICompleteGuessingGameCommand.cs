namespace CoreCodedChatbot.ApiApplication.Interfaces.Commands.GuessingGame
{
    public interface ICompleteGuessingGameCommand
    {
        void CompleteCurrentGuessingGame(decimal finalPercentage);
    }
}