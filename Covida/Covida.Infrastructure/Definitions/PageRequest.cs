using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Covida.Infrastructure.Definitions
{
    public class PageRequest<T> : IRequest<PageResult<T>> where T : new()
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 5;

        public async Task<PageResult<T>> GetResult(IQueryable<T> query)
        {
            var itemsTotal = await query.CountAsync();
            var items = await query.Skip(PageSize * (Page - 1)).Take(PageSize).ToArrayAsync();
            return new PageResult<T>()
            {
                Page = Page,
                PageSize = PageSize,
                Total = itemsTotal,
                Items = items,
            };
        }
    }
}
