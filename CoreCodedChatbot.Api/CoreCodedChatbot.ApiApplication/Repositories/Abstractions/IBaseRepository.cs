using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreCodedChatbot.ApiApplication.Repositories.Abstractions;

public interface IBaseRepository<TDbEntity> : IDisposable where TDbEntity : class
{
    IQueryable<TDbEntity> GetAll();
    Task<List<TDbEntity>> GetAllPagedAsync(int? page, int? pageSize, string? orderByColumnName, bool? desc, string? filterByColumn, object? filterValue);
    Task<TDbEntity> GetByIdAsync<TKeyType>(TKeyType id) where TKeyType : notnull;
    Task CreateAsync(TDbEntity entity);
    Task DeleteAsync<TKey>(TKey id);
    Task<TDbEntity?> GetByIdOrNullAsync<TKeyType>(TKeyType id) where TKeyType : notnull;
}