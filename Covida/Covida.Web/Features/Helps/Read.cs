using Covida.Core.Domain.Constants;
using Covida.Data.Postgre;
using Covida.Infrastructure.Definitions;
using Covida.Infrastructure.Exceptions;
using Covida.Infrastructure.Geometry;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Covida.Web.Features.Helps
{
    public class Read
    {
        public class Query : IRequest<Result>
        {
            public Guid Id { get; set; }
        }

        public class CommandValidator : AbstractValidator<Query>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Id).NotEmpty();
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
            public IEnumerable<HelpItemResult> HelpItems { get; set; }

            public class UserResult
            {
                public int Id { get; set; }
                public string Name { get; set; }
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

        public class RequestHandler : IRequestHandler<Query, Result>
        {
            private readonly CovidaDbContext db;

            public RequestHandler(CovidaDbContext db)
            {
                this.db = db;
            }

            public async Task<Result> Handle(Query request, CancellationToken cancellationToken)
            {
                // Get the help
                var help = await db.Helps
                    .Include(x => x.HelpItems)
                    .Select(x => new Result
                    {
                        Id = x.Id,
                        CancelledAt = x.CancelledAt,
                        CancelledReason = x.CancelledReason,
                        CreatedAt = x.CreatedAt,
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
                            Id = x.User.Id,
                            Name = x.User.Name,
                            Location = x.User.Location.ToPointD(),
                        },
                    })
                    .FirstOrDefaultAsync(x => x.Id == request.Id);
                // If it doesn't exist
                if (help is null)
                {
                    // Throw a Bad Request exception
                    throw new HttpException(HttpStatusCode.BadRequest, ErrorMessages.EntityNotFound<Core.Domain.Help>());
                }
                // Otherwise return
                return help;
            }
        }
    }
}
