using System.Collections.Generic;
using System.Linq;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.GuessingGame;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Bytes;
using CoreCodedChatbot.ApiApplication.Models.Intermediates;

namespace CoreCodedChatbot.ApiApplication.Commands.GuessingGame
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
            if (winners == null) return;

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