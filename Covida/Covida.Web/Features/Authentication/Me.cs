using Covida.Core.Domain;
using Covida.Data.Postgre;
using Covida.Infrastructure.Definitions;
using Covida.Infrastructure.Geometry;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Covida.Web.Features.Authentication
{
    public class Me
    {
        public class Query : IActorAwareRequest<Result>
        {
            public User Actor { get; set; }
        }

        public class Result
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public PointD Location { get; set; }
            public string Address { get; set; }
            public bool IsVolunteer { get; set; }
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
                // Map the user to Result
                var actor = request.Actor;
                var location = new PointD
                {
                    X = actor.Longitude,
                    Y = actor.Latitude,
                };
                var result = new Result
                {
                    Id = actor.Id,
                    Address = actor.Address,
                    IsVolunteer = actor.IsVolunteer,
                    Location = location,
                    Name = actor.Name,
                };
                // Return
                return await Task.FromResult(result);
            }
        }

    }

}
