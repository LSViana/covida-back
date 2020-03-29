using Covida.Core.Domain.Constants;
using Covida.Data.Postgre;
using Covida.Infrastructure.Definitions;
using Covida.Infrastructure.Geometry;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Covida.Web.Features.Helps
{
    public class List
    {
        public class Query : PageRequest<Result>
        {
            public float? X { get; set; }
            public float? Y { get; set; }
            public float MaxDistance { get; set; } = 10;
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(x => x.X).NotNull().InclusiveBetween(-180f, 180f);
                RuleFor(x => x.Y).NotNull().InclusiveBetween(-90f, 90f);
                RuleFor(x => x.MaxDistance).InclusiveBetween(1, 100);
            }
        }

        public class Result
        {
            public Guid Id { get; set; }
            public DateTime? CancelledAt { get; set; }
            public DateTime CreatedAt { get; set; }
            public string CancelledReason { get; set; }
            public HelpStatus HelpStatus { get; set; }
            public UserResult User { get; set; }
            public double CartesianDistance { get; set; }

            public class UserResult
            {
                public int Id { get; set; }
                public string Name { get; set; }
                public PointD Location { get; set; }
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
                var requestPoint = new Point(request.X.Value, request.Y.Value);
                requestPoint.SRID = Core.Domain.Constants.Location.DefaultSRID;
                // Get all helps
                var helpsQuery = db.Helps
                    .Include(x => x.User)
                    // Map them to result
                    .Select(x => new Result
                    {
                        Id = x.Id,
                        CancelledAt = x.CancelledAt,
                        CreatedAt = x.CreatedAt,
                        CancelledReason = x.CancelledReason,
                        HelpStatus = x.HelpStatus,
                        CartesianDistance = x.User.Location.Distance(requestPoint),
                        User = new Result.UserResult
                        {
                            Id = x.User.Id,
                            Name = x.User.Name,
                            Location = x.User.Location.ToPointD(),
                        },
                    })
                    .Where(x => x.CartesianDistance < request.MaxDistance);
                // Return the result
                var results = await request.GetResult(helpsQuery);
                return results;
            }
        }
    }
}
