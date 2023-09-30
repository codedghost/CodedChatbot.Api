using System.Collections.Generic;

namespace CoreCodedChatbot.ApiApplication.Repositories.Abstractions;

public class PagedResult<TDbEntity>
    where TDbEntity : class
{
    public IEnumerable<TDbEntity> Result { get; set; }
    public int Total { get; set; }
}