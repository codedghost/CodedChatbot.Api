using System;
using CoreCodedChatbot.ApiContract.RequestModels.GuessingGame;
using CoreCodedChatbot.Library.Interfaces.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.KeyVault.Models;
using Microsoft.Extensions.Logging;

namespace CoreCodedChatbot.Api.Controllers
{
    [Route("GuessingGame/[action]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class GuessingGameController : Controller
    {
        private IGuessingGameService _guessingGameService;
        private readonly ILogger<GuessingGameController> _logger;

        private object timerLock = new object();

        public GuessingGameController(
            IGuessingGameService guessingGameService,
            ILogger<GuessingGameController> logger
            )
        {
            _guessingGameService = guessingGameService;
            _logger = logger;
        }

        [HttpPost]
        public IActionResult StartGuessingGame([FromBody] StartGuessingGameRequest songInfo)
        {
            try
            {
                bool isGameInProgress;
                lock (timerLock)
                {
                    // Check guessing game state
                    isGameInProgress = _guessingGameService.IsGuessingGameInProgress();
                }

                if (!isGameInProgress)
                    _guessingGameService.GuessingGameStart(songInfo.SongName, songInfo.SongLengthSeconds);

                return Ok();
            }
            catch (Exception e)
            {

                _logger.LogError(e, "Error in StartGuessingGame");
                return BadRequest();
            }
        }

        [HttpPost]
        public IActionResult FinishGuessingGame([FromBody] decimal finalPercentage)
        {
            if (_guessingGameService.SetPercentageAndFinishGame(finalPercentage))
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpPost]
        public IActionResult SubmitGuess([FromBody] SubmitGuessRequest submitGuessModel)
        {
            if (_guessingGameService.UserGuess(submitGuessModel.Username, submitGuessModel.Guess))
                return Ok();

            return BadRequest();
        }
    }
}
