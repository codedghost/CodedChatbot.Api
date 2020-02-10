﻿using System.Collections.Generic;
using CoreCodedChatbot.Api.Models.Intermediates;

namespace CoreCodedChatbot.Api.Interfaces.Commands.GuessingGame
{
    public interface IGiveGuessingGameWinnersBytesCommand
    {
        void Give(List<GuessingGameWinner> winners);
    }
}