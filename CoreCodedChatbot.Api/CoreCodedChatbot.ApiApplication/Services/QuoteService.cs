using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CoreCodedChatbot.ApiApplication.Interfaces.Services;
using CoreCodedChatbot.ApiApplication.Repositories.Abstractions;
using CoreCodedChatbot.ApiApplication.Repositories.Quotes;
using CoreCodedChatbot.ApiContract.ResponseModels.Quotes;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;
using Microsoft.EntityFrameworkCore;
using ApiQuote = CoreCodedChatbot.ApiContract.ResponseModels.Quotes.ChildModels.Quote;

namespace CoreCodedChatbot.ApiApplication.Services;

public class QuoteService : IBaseService, IQuoteService
{
    private readonly IChatbotContextFactory _chatbotContextFactory;
    private readonly IChatService _chatService;
    private readonly IMapper _mapper;
    private readonly Random _random;

    public QuoteService(
        IChatbotContextFactory chatbotContextFactory,
        IChatService chatService,
        IMapper mapper)
    {
        _chatbotContextFactory = chatbotContextFactory;
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

        using (var repo = new QuotesRepository(_chatbotContextFactory))
        {
            await repo.CreateAndSaveAsync(newEntity);
        }

        return newEntity.QuoteId;
    }

    public async Task EditQuote(int quoteId, string quoteText, string username, bool isMod)
    {
        using (var repo = new QuotesRepository(_chatbotContextFactory))
        {
            await repo.Edit(quoteId, quoteText, username, isMod);
        }
    }

    public async Task RemoveQuote(int quoteId, string username, bool isMod)
    {
        using (var repo = new QuotesRepository(_chatbotContextFactory))
        {
            await repo.Archive(quoteId, username, isMod);
        }
    }

    public async Task<ApiQuote> GetQuote(int? quoteId)
    {
        Quote quote;
        if (quoteId == null)
        {
            quote = await GetRandomQuote();
        }
        else
        {
            using (var repo = new QuotesRepository(_chatbotContextFactory))
            {
                quote = await repo.GetByIdOrNullAsync(quoteId.Value);
            }
        }

        return _mapper.Map<ApiQuote>(quote);
    }
        
    public async Task<GetQuotesResponse> GetQuotes(int? page, int? pageSize, string? orderByColumnName, bool? desc, string? filterByColumn, object? filterValue)
    {
        PagedResult<Quote> pagedResult;
        using (var repo = new QuotesRepository(_chatbotContextFactory))
        {
            pagedResult =
                await repo.GetAllPagedAsync(page, pageSize, orderByColumnName, desc, filterByColumn, filterValue);
        }

        return new GetQuotesResponse
        {
            Quotes = _mapper.Map<List<ApiQuote>>(pagedResult.Result),
            Total = pagedResult.Total
        };
    }

    private async Task<Quote> GetRandomQuote()
    {
        using (var repo = new QuotesRepository(_chatbotContextFactory))
        {
            var quotesQuery = repo.GetAll().Where(q => q.Enabled).OrderBy(q => q.QuoteId);

            var count = quotesQuery.Count();

            var randomInt = _random.Next(count);

            return await quotesQuery.Skip(randomInt).Take(1).FirstOrDefaultAsync();
        }
    }

    public async Task<bool> SendQuoteToChat(int id, string username)
    {
        var quote = await GetQuote(id);

        if (quote == null) return false;

        return await _chatService.SendMessage($"Hey @{username}, Here is Quote {id}: {quote.QuoteText}");
    }
}