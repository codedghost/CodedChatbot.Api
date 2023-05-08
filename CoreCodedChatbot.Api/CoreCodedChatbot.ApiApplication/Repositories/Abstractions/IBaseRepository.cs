using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreCodedChatbot.ApiApplication.Repositories.Abstractions;

public interface IBaseRepository<TDbEntity> : IDisposable where TDbEntity : class
{
    IQueryable<TDbEntity> GetAll();
    Task<List<TDbEntity>> GetAllPagedAsync(int page = 0, int pageSize = 50, string orderByColumnName = "", bool desc = false);
    Task<TDbEntity> GetByIdAsync<TKeyType>(TKeyType id) where TKeyType : notnull;
    Task CreateAsync(TDbEntity entity);
    Task DeleteAsync<TKey>(TKey id);
}