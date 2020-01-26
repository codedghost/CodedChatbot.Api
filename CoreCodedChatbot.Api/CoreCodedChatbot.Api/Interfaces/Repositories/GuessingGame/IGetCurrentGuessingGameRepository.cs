using CoreCodedChatbot.Database.Context.Models;

namespace CoreCodedChatbot.Api.Interfaces.Repositories.GuessingGame
{
    public interface IGetCurrentGuessingGameRepository
    {
        SongGuessingRecord Get();
    }
}