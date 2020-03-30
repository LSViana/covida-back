using Covida.Core.Domain;
using Covida.Data.Postgre;
using Covida.Infrastructure.Definitions;
using Covida.Infrastructure.Exceptions;
using Covida.Web.Hubs;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Covida.Web.Features.Helps
{
    public class FinishHelp
    {
        public class Command : IRequest<Unit>
        {
            public Guid HelpId { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.HelpId).NotNull();
            }
        }

        public class RequestHandler : IRequestHandler<Command, Unit>
        {
            private readonly CovidaDbContext db;
            private readonly IHubContext<MessagesHub> hubContext;

            public RequestHandler(CovidaDbContext db, IHubContext<MessagesHub> hubContext)
            {
                this.db = db;
                this.hubContext = hubContext;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                // Get help and verify if it exists
                var help = await db.Helps
                    .Where(x => x.HelpStatus == Core.Domain.Constants.HelpStatus.Active)
                    .FirstOrDefaultAsync(x => x.Id == request.HelpId);
                if (help is null)
                {
                    throw new HttpException(HttpStatusCode.BadRequest, ErrorMessages.EntityNotFound<Help>());
                }
                // Set finished
                help.HelpStatus = Core.Domain.Constants.HelpStatus.Past;
                // Save to database
                db.Helps.Update(help);
                await db.SaveChangesAsync(cancellationToken);
                // Send WebSocket message
                await hubContext.Clients.User(help.AuthorId.ToString()).SendCoreAsync("helpFinished", new object[] { help.Id }, cancellationToken);
                // Return result
                return Unit.Value;
            }
        }

    }

}
