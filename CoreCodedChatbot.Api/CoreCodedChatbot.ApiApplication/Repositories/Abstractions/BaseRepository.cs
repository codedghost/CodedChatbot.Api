using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using CoreCodedChatbot.Database.Context.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CoreCodedChatbot.ApiApplication.Repositories.Abstractions;

public abstract class BaseRepository<TDbEntity> : IBaseRepository<TDbEntity> where TDbEntity : class
{
    public readonly IChatbotContext _context;

    public BaseRepository(IChatbotContextFactory chatbotContextFactory)
    {
        _context = chatbotContextFactory.Create();

        if (typeof(TDbEntity).GetInterfaces().All(i => i.Name != nameof(IEntity)))
        {
            throw new ArgumentException(
                $"Given TDbSet Type: {nameof(TDbEntity)} does not implement the required IEntity interface");
        }
    }

    public IQueryable<TDbEntity> GetAll()
    {
        var dbSet = _context.Set<TDbEntity>();

        return dbSet;
    }

    public async Task<List<TDbEntity>> GetAllPagedAsync(
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

        var dbSet = _context.Set<TDbEntity>();

        if (string.IsNullOrWhiteSpace(orderByColumnName))
            orderByColumnName = DbSetExtensions.GetKeyField(typeof(TDbEntity));

        var query = filterByColumn == null ? dbSet.AsQueryable() : dbSet.FilterBy(filterByColumn, filterValue);
        //query = !descValue ? query.OrderBy(orderByColumnName) : query.OrderByDescending(orderByColumnName);

        query = query.Skip(pageValue * pageSizeValue).Take(pageSizeValue);

        return await query.ToListAsync();
    }

    public async Task<TDbEntity> GetByIdAsync<TKeyType>(TKeyType id) where TKeyType : notnull
    {
        var dbSet = _context.Set<TDbEntity>();

        var result = await dbSet.FindAsync(id);

        return result ?? throw new KeyNotFoundException($"{nameof(TDbEntity)}Entity not found with id: {id}");
    }

    public async Task CreateAsync(TDbEntity entity)
    {
        _context.Set<TDbEntity>().Add(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync<TKey>(TKey id)
    {
        var dbSet = _context.Set<TDbEntity>();

        var entity = await dbSet.FindAsync(id);

        if (entity != null)
        {
            dbSet.Remove(entity);
        }

        await _context.SaveChangesAsync();
    }

    public void Dispose() => _context.Dispose();
}