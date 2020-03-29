using Covida.Core.Domain;
using Covida.Data.Postgre;
using Covida.Infrastructure.Definitions;
using Covida.Infrastructure.Exceptions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Covida.Web.Features.Authentication
{
    public class Login
    {
        public class Command : IRequest<GenerateToken.Result>
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Id).GreaterThan(0);
                RuleFor(x => x.Name).NotNull().Length(2, 64);
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
                // Get user
                var user = await db.Users.FirstOrDefaultAsync(x => x.Id == request.Id && x.Name == request.Name);
                // If user is null
                if(user is null)
                {
                    // Return Bad Request to client
                    throw new HttpException(HttpStatusCode.BadRequest, ErrorMessages.EntityNotFound<User>());
                }
                // Otherwise, return the generated token
                return await mediator.Send(new GenerateToken.Query { User = user });
            }
        }

    }

}
