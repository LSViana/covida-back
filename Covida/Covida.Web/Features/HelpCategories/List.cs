using Covida.Data.Postgre;
using Covida.Infrastructure.Definitions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Covida.Web.Features.HelpCategories
{
    public class List
    {
        public class Query : PageRequest<Result>
        {
        }

        public class Result
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public class RequestHandler : IRequestHandler<Query, PageResult<Result>>
        {
            private readonly CovidaDbContext db;

            public RequestHandler(CovidaDbContext db)
            {
                this.db = db;
            }

            public async Task<PageResult<Result>> Handle(Query request, CancellationToken cancellationToken)
            {
                // Query all help categories mapped to result
                var helpCategoriesQuery = db.HelpCategories
                    .Select(x => new Result
                    {
                        Id = x.Id,
                        Name = x.Name,
                    });
                // Return result
                return await request.GetResult(helpCategoriesQuery);
            }
        }

    }

}
