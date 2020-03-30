using Covida.Core.Domain;
using Covida.Core.Domain.Constants;
using Covida.Data.Postgre;
using Covida.Infrastructure.Definitions;
using Covida.Infrastructure.Geometry;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Covida.Web.Features.Helps
{
    public class Mine
    {
        public class Query : ActorAwarePageRequest<Result>
        {
        }

        public class Result
        {
            public Guid Id { get; set; }
            public DateTime? CancelledAt { get; set; }
            public DateTime CreatedAt { get; set; }
            public string CancelledReason { get; set; }
            public HelpStatus HelpStatus { get; set; }
            public UserResult User { get; set; }
            public string[] Categories { get; set; }
            public IEnumerable<HelpItemResult> HelpItems { get; set; }

            public class UserResult
            {
                public int Id { get; set; }
                public string Name { get; set; }
                public string Address { get; set; }
                public PointD Location { get; set; }
            }

            public class HelpItemResult
            {
                public Guid Id { get; set; }
                public string Name { get; set; }
                public uint Amount { get; set; }
                public bool Complete { get; set; }
            }
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
                // Get all helps for this user that are active
                var helpsQuery = db.Helps
                    .Where(x => x.VolunteerId == request.Actor.Id || x.AuthorId == request.Actor.Id)
                    // Map them to result
                    .Select(x => new Result
                    {
                        Id = x.Id,
                        CancelledAt = x.CancelledAt,
                        CancelledReason = x.CancelledReason,
                        CreatedAt = x.CreatedAt,
                        Categories = x.HelpHasCategories.Select(y => y.HelpCategory.Name).ToArray(),
                        HelpItems = x.HelpItems.Select(y => new Result.HelpItemResult
                        {
                            Id = y.Id,
                            Name = y.Name,
                            Amount = y.Amount,
                            Complete = y.Complete,
                        }),
                        HelpStatus = x.HelpStatus,
                        User = new Result.UserResult
                        {
                            Id = x.Author.Id,
                            Name = x.Author.Name,
                            Address = x.Author.Address,
                            Location = x.Author.Location.ToPointD(),
                        },
                    });
                // Return the results
                return await request.GetResult(helpsQuery);
            }
        }
    }

}
