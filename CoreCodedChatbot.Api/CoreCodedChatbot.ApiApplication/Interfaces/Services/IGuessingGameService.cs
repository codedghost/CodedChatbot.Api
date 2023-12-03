using System.Threading.Tasks;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Services;

public interface IGuessingGameService
{
    Task GuessingGameStart(string songName, int songLengthInSeconds);
    Task<bool> SetPercentageAndFinishGame(decimal finalPercentage);
    Task<bool> SubmitOrUpdateGuess(string username, decimal percentageGuess);
    bool IsGuessingGameInProgress();
    Task<bool> SetGuessingGameState(bool state);
}