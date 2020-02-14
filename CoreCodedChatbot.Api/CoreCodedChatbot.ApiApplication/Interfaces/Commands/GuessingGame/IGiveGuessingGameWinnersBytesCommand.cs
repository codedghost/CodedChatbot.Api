using System.Collections.Generic;
using CoreCodedChatbot.ApiApplication.Models.Intermediates;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Commands.GuessingGame
{
    public interface IGiveGuessingGameWinnersBytesCommand
    {
        void Give(List<GuessingGameWinner> winners);
    }
}