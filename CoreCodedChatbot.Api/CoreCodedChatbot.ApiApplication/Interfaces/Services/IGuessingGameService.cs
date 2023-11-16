using System.Threading.Tasks;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Services;

public interface IGuessingGameService
{
    Task GuessingGameStart(string songName, int songLengthInSeconds);
    bool SetPercentageAndFinishGame(decimal finalPercentage);
    bool SubmitOrUpdateGuess(string username, decimal percentageGuess);
    bool IsGuessingGameInProgress();
    bool SetGuessingGameState(bool state);
}