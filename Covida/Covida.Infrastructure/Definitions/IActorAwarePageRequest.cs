using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Covida.Infrastructure.Definitions
{
    public interface IActorAwarePageRequest<T> : IActorAwareRequest<PageResult<T>> where T : new()
    {
        int Page { get; set; }
        int PageSize { get; set; }

        Task<PageResult<T>> GetResult(IQueryable<T> query);
    }
}
