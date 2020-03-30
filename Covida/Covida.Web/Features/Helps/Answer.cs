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
    public class Answer
    {
        public class Command : IActorAwareRequest<Unit>
        {
            public Guid HelpId { get; set; }
            public User Actor { get; set; }
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
                // Get help
                var help = await db.Helps.FirstOrDefaultAsync(x => x.Id == request.HelpId);
                // If it doesn't exist, throw BadRequest
                if(help is null)
                {
                    throw new HttpException(HttpStatusCode.BadRequest, ErrorMessages.EntityNotFound<Help>());
                }
                // If there is an assigned volunteer, throw BadRequest
                if(help.Volunteer != null)
                {
                    throw new HttpException(HttpStatusCode.BadRequest, ErrorMessages.AnotherVolunteerAlreadyAnswered);
                }
                // Otherwise, assign the volunteer and return
                help.Volunteer = request.Actor;
                help.HelpStatus = Core.Domain.Constants.HelpStatus.Active;
                db.Helps.Update(help);
                await db.SaveChangesAsync();
                // Send the message that this help is taken
                await hubContext.Clients.Group(MessagesHub.VolunteerGroupName).SendCoreAsync("helpAnswered", new object[] { help.Id }, cancellationToken);
                // Return result
                return Unit.Value;
            }
        }
    }
}
