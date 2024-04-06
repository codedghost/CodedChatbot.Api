using System;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Interfaces.Services;
using CoreCodedChatbot.ApiContract.RequestModels.Counters;
using CoreCodedChatbot.ApiContract.ResponseModels.Counters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoreCodedChatbot.Api.Controllers;

[Route("Counters/[action]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class CounterController : Controller
{
    private readonly ICounterService _counterService;
    private readonly ILogger<CounterController> _logger;

    public CounterController(
        ICounterService counterService,
        ILogger<CounterController> logger)
    {
        _counterService = counterService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetCounters(int? page, int? pageSize, string? orderByColumnName, bool? desc,
        string? filterByColumn, string? filterByValue)
    {
        try
        {
            var counters = await _counterService.GetCounters(page, pageSize, orderByColumnName, desc,
                filterByColumn, filterByValue);

            return Json(new GetCountersResponse
            {
                Counters = counters
            });
        }
        catch (Exception e)
        {
            _logger.Log(LogLevel.Error, e, "Error when retrieving Counter list");
            return BadRequest();
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetCounter(string counterName)
    {
        try
        {
            var counter = await _counterService.GetCounter(counterName);

            return Json(new GetCounterResponse
            {
                Counter = counter
            });
        }
        catch (Exception e)
        {
            _logger.Log(LogLevel.Error, e, $"Error when retrieving counter: {counterName}");
            return NotFound();
        }
    }

    [HttpPut]
    public async Task<IActionResult> AddCounter([FromBody] CreateCounterRequest request)
    {
        try
        {
            await _counterService.CreateCounter(request.CounterName, request.CounterPreText,
                request.CounterInitialVal);

            return Ok();
        }
        catch (Exception e)
        {
            _logger.Log(LogLevel.Error, e, $"Error when creating Counter: {request.CounterName}",
                new object[] {request.CounterName, request.CounterPreText, request.CounterInitialVal});
            return BadRequest();
        }
    }

    [HttpGet]
    public async Task<IActionResult> UpdateCounter(string counterName)
    {
        try
        {
            var counter = await _counterService.UpdateCounter(counterName);

            return Json(new UpdateCounterResponse
            {
                Counter = counter
            });
        }
        catch (Exception e)
        {
            _logger.Log(LogLevel.Error, e, $"Error when updating Counter: {counterName}");
            return BadRequest();
        }
    }

    [HttpPost]
    public async Task<IActionResult> Reset([FromBody] ResetCounterRequest request)
    {
        try
        {
            await _counterService.ResetCounter(request.CounterName);

            return Ok();
        }
        catch (Exception e)
        {
            _logger.Log(LogLevel.Error, e, $"Error when Resetting Counter: {request.CounterName}");
            return BadRequest();
        }
    }

    [HttpPost]
    public async Task<IActionResult> UpdateSuffix([FromBody] UpdateCounterSuffixRequest request)
    {
        try
        {
            await _counterService.UpdateCounterSuffix(request.CounterName, request.CounterSuffix);

            return Ok();
        }
        catch (Exception e)
        {
            _logger.Log(LogLevel.Error, e, $"Error when Resetting Counter Suffix: {request.CounterName}");
            return BadRequest();
        }
    }
}