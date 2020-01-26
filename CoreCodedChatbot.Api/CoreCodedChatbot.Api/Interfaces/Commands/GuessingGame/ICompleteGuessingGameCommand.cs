namespace CoreCodedChatbot.Api.Interfaces.Commands.GuessingGame
{
    public interface ICompleteGuessingGameCommand
    {
        void CompleteCurrentGuessingGame(decimal finalPercentage);
    }
}