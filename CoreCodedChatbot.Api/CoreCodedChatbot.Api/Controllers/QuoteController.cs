﻿using System;
using System.Linq;
using CoreCodedChatbot.ApiApplication.Interfaces.Services;
using CoreCodedChatbot.ApiContract.RequestModels.Quotes;
using CoreCodedChatbot.ApiContract.ResponseModels.Quotes;
using CoreCodedChatbot.ApiContract.ResponseModels.Quotes.ChildModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoreCodedChatbot.Api.Controllers
{
    [Route("Quote/[action]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class QuoteController : Controller
    {
        private readonly IQuoteService _quoteService;
        private readonly ILogger<QuoteController> _logger;

        public QuoteController(
            IQuoteService quoteService,
            ILogger<QuoteController> logger
            )
        {
            _quoteService = quoteService;
            _logger = logger;
        }

        [HttpPut]
        public IActionResult AddQuote([FromBody] AddQuoteRequest addRequest)
        {
            try
            {
                var quoteId = _quoteService.AddQuote(addRequest.Username, addRequest.QuoteText);

                return Json(new AddQuoteResponse
                {
                    QuoteId = quoteId
                });
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Error, e, "Error when adding quote", addRequest);
                return BadRequest();
            }
        }

        [HttpPost]
        public IActionResult EditQuote([FromBody] EditQuoteRequest editRequest)
        {
            try
            {
                _quoteService.EditQuote(editRequest.QuoteId, editRequest.QuoteText, editRequest.Username, editRequest.IsMod);

                return Ok();
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Error, e, "Error when editing quote", editRequest);
                return BadRequest();
            }
        }

        [HttpPost]
        public IActionResult RemoveQuote([FromBody] RemoveQuoteRequest removeRequest)
        {
            try
            {
                _quoteService.RemoveQuote(removeRequest.QuoteId, removeRequest.Username, removeRequest.IsMod);

                return Ok();
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Error, e, "Error when removing quote", removeRequest);
                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult GetQuote(int? quoteId)
        {
            try
            {
                var quote = _quoteService.GetQuote(quoteId);

                return Json(new GetQuoteResponse
                { 
                    Quote = new Quote
                    {
                        QuoteId = quote.QuoteId,
                        QuoteText = quote.QuoteText
                    }
                });
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Error, e, "Error when retrieving quote", quoteId);
                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult GetQuotes()
        {
            try
            {
                var quotes = _quoteService.GetQuotes();

                return Json(new GetQuotesResponse
                {
                    Quotes = quotes.Select(q => new Quote
                    {
                        QuoteId = q.QuoteId,
                        QuoteText = q.QuoteText,
                        CreatedBy = q.CreatedBy,
                        Disabled = q.Disabled,
                        LastEditedBy = q.EditedBy,
                        EditedAt = q.EditedAt
                    }).ToList()
                });
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Error, e, "Error when retrieving quote list");
                return BadRequest();
            }
        }
    }
}