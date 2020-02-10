using System.Collections.Generic;
using System.Linq;
using CoreCodedChatbot.Api.Interfaces.Commands.GuessingGame;
using CoreCodedChatbot.Api.Interfaces.Repositories.Bytes;
using CoreCodedChatbot.Api.Models.Intermediates;

namespace CoreCodedChatbot.Api.Commands.GuessingGame
{
    public class GiveGuessingGameWinnersBytesCommand : IGiveGuessingGameWinnersBytesCommand
    {
        private readonly IGiveUsersBytesRepository _giveUsersBytesRepository;

        public GiveGuessingGameWinnersBytesCommand(
            IGiveUsersBytesRepository giveUsersBytesRepository
            )
        {
            _giveUsersBytesRepository = giveUsersBytesRepository;
        }

        public void Give(List<GuessingGameWinner> winners)
        {
            var bytesModel = winners.Select(w =>
                new GiveBytesToUserModel
                {
                    Username = w.Username,
                    Bytes = w.BytesWon
                }).ToList();

            _giveUsersBytesRepository.GiveBytes(bytesModel);
        }
    }
}