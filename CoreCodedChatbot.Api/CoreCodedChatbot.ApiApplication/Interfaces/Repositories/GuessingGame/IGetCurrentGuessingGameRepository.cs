using CoreCodedChatbot.Database.Context.Models;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Repositories.GuessingGame;

public interface IGetCurrentGuessingGameRepository
{
    SongGuessingRecord Get();
}