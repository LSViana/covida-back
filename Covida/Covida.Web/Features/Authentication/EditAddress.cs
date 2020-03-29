using Covida.Core.Domain;
using Covida.Data.Postgre;
using Covida.Infrastructure.Definitions;
using Covida.Infrastructure.Geometry;
using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Covida.Web.Features.Authentication
{
    public class EditAddress
    {
        public class Command : IActorAwareRequest<Unit>
        {
            public PointD Location { get; set; }
            public string Address { get; set; }
            public User Actor { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Address).NotNull().Length(6, 64);
                RuleFor(x => x.Location.X).InclusiveBetween(-180f, 180f);
                RuleFor(x => x.Location.Y).InclusiveBetween(-90f, 90f);
            }
        }

        public class RequestHandler : IRequestHandler<Command, Unit>
        {
            private readonly CovidaDbContext db;

            public RequestHandler(CovidaDbContext db)
            {
                this.db = db;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                // Update its location and address
                request.Actor.Address = request.Address;
                request.Actor.Location = request.Location.ToPoint();
                // Save to database
                db.Users.Update(request.Actor);
                await db.SaveChangesAsync();
                // Return result
                return Unit.Value;
            }
        }
    }

}
