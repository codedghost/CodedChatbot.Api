using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreCodedChatbot.Database.Context.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CoreCodedChatbot.ApiApplication.Repositories.Abstractions;

public abstract class BaseRepository<TDbEntity> : IBaseRepository<TDbEntity> where TDbEntity : class
{
    public readonly IChatbotContext Context;

    public BaseRepository(IChatbotContextFactory chatbotContextFactory)
    {
        Context = chatbotContextFactory.Create();

        if (typeof(TDbEntity).GetInterfaces().All(i => !i.IsAssignableTo(typeof(IEntity))))
        {
            throw new ArgumentException(
                $"Given TDbSet Type: {nameof(TDbEntity)} does not implement the required IEntity interface");
        }
    }

    public IQueryable<TDbEntity> GetAll()
    {
        var dbSet = Context.Set<TDbEntity>();

        return dbSet;
    }

    public async Task<PagedResult<TDbEntity>> GetAllPagedAsync(
        int? page,
        int? pageSize,
        string? orderByColumnName,
        bool? desc,
        string? filterByColumn,
        object? filterValue)
    {
        var pageValue = page ?? 0;
        var pageSizeValue = pageSize ?? 50;
        var descValue = desc ?? false;

        var dbSet = Context.Set<TDbEntity>();

        if (string.IsNullOrWhiteSpace(orderByColumnName))
            orderByColumnName = DbSetExtensions.GetKeyField(typeof(TDbEntity));

        var query = filterByColumn == null ? dbSet.AsQueryable() : dbSet.FilterBy(filterByColumn, filterValue);
        query = !descValue ? query.OrderBy(orderByColumnName) : query.OrderByDescending(orderByColumnName);

        var total = await query.CountAsync();

        query = query.Skip(pageValue * pageSizeValue).Take(pageSizeValue);

        return new PagedResult<TDbEntity>
        {
            Result = await query.ToListAsync(),
            Total = total
        };
    }

    public async Task<TDbEntity> GetByIdAsync<TKeyType>(TKeyType id) where TKeyType : notnull
    {
        return await GetByIdOrNullAsync(id) ?? throw new KeyNotFoundException($"{nameof(TDbEntity)}Entity not found with id: {id}");
    }

    public async Task<TDbEntity?> GetByIdOrNullAsync<TKeyType>(TKeyType id) where TKeyType : notnull
    {
        var dbSet = Context.Set<TDbEntity>();

        var result = await dbSet.FindAsync(id);

        return result;
    }

    public async Task CreateAsync(TDbEntity entity)
    {
        await Context.Set<TDbEntity>().AddAsync(entity);
    }

    public async Task CreateAndSaveAsync(TDbEntity entity)
    {
        await CreateAsync(entity);
        await Context.SaveChangesAsync();
    }

    public async Task DeleteAsync<TKey>(TKey id)
    {
        var dbSet = Context.Set<TDbEntity>();

        var entity = await dbSet.FindAsync(id);

        if (entity != null)
        {
            dbSet.Remove(entity);
        }

        await Context.SaveChangesAsync();
    }

    public void Dispose() => Context.Dispose();
}