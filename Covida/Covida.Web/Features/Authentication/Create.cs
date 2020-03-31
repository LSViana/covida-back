using Covida.Core.Domain;
using Covida.Infrastructure.Definitions;
using Covida.Data.Postgre;
using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Covida.Infrastructure.Geometry;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Covida.Web.Features.Authentication
{
    public class Create
    {
        public class Command : IRequest<GenerateToken.Result>
        {
            public string Name { get; set; }
            public string Address { get; set; }
            public PointD Location { get; set; }
            public bool IsVolunteer { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Name).NotNull().Length(2, 64);
                RuleFor(x => x.Address).NotNull().Length(6, 64);
                RuleFor(x => x.Location.X).InclusiveBetween(-180f, 180f);
                RuleFor(x => x.Location.Y).InclusiveBetween(-90f, 90f);
            }
        }

        public class RequestHandler : IRequestHandler<Command, GenerateToken.Result>
        {
            private readonly CovidaDbContext db;
            private readonly IMediator mediator;

            public RequestHandler(CovidaDbContext db, IMediator mediator)
            {
                this.db = db;
                this.mediator = mediator;
            }

            public async Task<GenerateToken.Result> Handle(Command request, CancellationToken cancellationToken)
            {
                // Create the user
                var user = new User
                {
                    Id = db.Users.IgnoreQueryFilters().Max(x => x.Id) + 1,
                    Name = request.Name,
                    Longitude = request.Location.X,
                    Latitude = request.Location.Y,
                    Address = request.Address,
                    IsVolunteer = request.IsVolunteer,
                    CreatedAt = DateTime.Now,
                };
                // Save the user to the database
                await db.Users.AddAsync(user, cancellationToken);
                await db.SaveChangesAsync(cancellationToken);
                // Generate token and send to client
                return await mediator.Send(new GenerateToken.Query { User = user });
            }
        }

    }

}
