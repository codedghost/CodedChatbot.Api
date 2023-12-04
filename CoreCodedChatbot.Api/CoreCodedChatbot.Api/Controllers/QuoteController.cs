using System;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Interfaces.Services;
using CoreCodedChatbot.ApiContract.RequestModels.Quotes;
using CoreCodedChatbot.ApiContract.ResponseModels.Quotes;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoreCodedChatbot.Api.Controllers;

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
    public async Task<IActionResult> AddQuote([FromBody] AddQuoteRequest addRequest)
    {
        try
        {
            var quoteId = await _quoteService.AddQuote(addRequest.Username, addRequest.QuoteText);

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
    public async Task<IActionResult> EditQuote([FromBody] EditQuoteRequest editRequest)
    {
        try
        {
            await _quoteService.EditQuote(editRequest.QuoteId, editRequest.QuoteText, editRequest.Username, editRequest.IsMod);

            return Ok();
        }
        catch (Exception e)
        {
            _logger.Log(LogLevel.Error, e, "Error when editing quote", editRequest);
            return BadRequest();
        }
    }

    [HttpPost]
    public async Task<IActionResult> RemoveQuote([FromBody] RemoveQuoteRequest removeRequest)
    {
        try
        {
            await _quoteService.RemoveQuote(removeRequest.QuoteId, removeRequest.Username, removeRequest.IsMod);

            return Ok();
        }
        catch (Exception e)
        {
            _logger.Log(LogLevel.Error, e, "Error when removing quote", removeRequest);
            return BadRequest();
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetQuote(int? quoteId)
    {
        try
        {
            var quote = await _quoteService.GetQuote(quoteId);

            return Json(new GetQuoteResponse
            { 
                Quote = quote
            });
        }
        catch (Exception e)
        {
            _logger.Log(LogLevel.Error, e, "Error when retrieving quote", quoteId);
            return BadRequest();
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetQuotes(int? page, int? pageSize, string? orderByColumnName, bool? desc, string? filterByColumnName, string? filterByValue)
    {
        try
        {
            var quotesResponse = await _quoteService.GetQuotes(page, pageSize, orderByColumnName, desc, filterByColumnName, filterByValue);

            return Json(quotesResponse);
        }
        catch (Exception e)
        {
            _logger.Log(LogLevel.Error, e, "Error when retrieving quote list");
            return BadRequest();
        }
    }

    [HttpPost]
    public async Task<IActionResult> SendQuoteToChat([FromBody] SendQuoteToChatRequest request)
    {
        try
        {
            var success = await _quoteService.SendQuoteToChat(request.QuoteId, request.Username);

            return Json(success);
        }
        catch (Exception e)
        {
            _logger.Log(LogLevel.Error, e, "Error when sending message to chat");
            return BadRequest();
        }
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteQuote(int quoteId, string username)
    {
        try
        {
            await _quoteService.RemoveQuote(quoteId, username, true);

            return Ok();
        }
        catch (Exception e)
        {
            _logger.Log(LogLevel.Error, e, "Error when archiving quote");
            return BadRequest();
        }
    }
}