using System;
using System.Collections.Generic;
using System.Text;

namespace Covida.Infrastructure.Definitions
{
    public class PageResult<T> where T : new()
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }
        public IEnumerable<T> Items { get; set; }
    }
}
