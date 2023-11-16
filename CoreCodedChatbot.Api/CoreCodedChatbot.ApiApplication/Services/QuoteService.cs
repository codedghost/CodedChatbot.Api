using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Quote;
using CoreCodedChatbot.ApiApplication.Interfaces.Services;
using CoreCodedChatbot.ApiContract.ResponseModels.Quotes;
using CoreCodedChatbot.Database.Context.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.VisualStudio.Services.GitHubConnector;
using ApiQuote = CoreCodedChatbot.ApiContract.ResponseModels.Quotes.ChildModels.Quote;

namespace CoreCodedChatbot.ApiApplication.Services;

public class QuoteService : IQuoteService
{
    private readonly IQuoteRepository _quoteRepository;
    private readonly IChatService _chatService;
    private readonly IMapper _mapper;
    private readonly Random _random;

    public QuoteService(
        IQuoteRepository quoteRepository,
        IChatService chatService,
        IMapper mapper)
    {
        _quoteRepository = quoteRepository;
        _chatService = chatService;
        _mapper = mapper;

        _random = new Random();
    }

    public async Task<int> AddQuote(string username, string quoteText)
    {
        var newEntity = new Quote
        {
            Username = username,
            QuoteText = quoteText,
            Enabled = true,
            LastEdited = DateTime.UtcNow
        };

        await _quoteRepository.CreateAsync(newEntity);

        return newEntity.QuoteId;
    }

    public async Task EditQuote(int quoteId, string quoteText, string username, bool isMod)
    {
        await _quoteRepository.Edit(quoteId, quoteText, username, isMod);
    }

    public async Task RemoveQuote(int quoteId, string username, bool isMod)
    {
        await _quoteRepository.Archive(quoteId, username, isMod);
    }

    public async Task<ApiQuote> GetQuote(int? quoteId)
    {
        var quote = quoteId == null ? await GetRandomQuote() : await _quoteRepository.GetByIdAsync(quoteId.Value);

        return _mapper.Map<ApiQuote>(quote);
    }
        
    public async Task<GetQuotesResponse> GetQuotes(int? page, int? pageSize, string? orderByColumnName, bool? desc, string? filterByColumn, object? filterValue)
    {
        var pagedResult = await _quoteRepository.GetAllPagedAsync(page, pageSize, orderByColumnName, desc, filterByColumn, filterValue);

        return new GetQuotesResponse
        {
            Quotes = _mapper.Map<List<ApiQuote>>(pagedResult.Result),
            Total = pagedResult.Total
        };
    }

    private async Task<Quote> GetRandomQuote()
    {
        var quotesQuery = _quoteRepository.GetAll().Where(q => q.Enabled).OrderBy(q => q.QuoteId);

        var count = quotesQuery.Count();

        var randomInt = _random.Next(count);

        return await quotesQuery.Skip(randomInt).Take(1).FirstOrDefaultAsync();
    }

    public async Task<bool> SendQuoteToChat(int id, string username)
    {
        var quote = await GetQuote(id);

        if (quote == null) return false;

        return await _chatService.SendMessage($"Hey @{username}, Here is Quote {id}: {quote.QuoteText}");
    }
}