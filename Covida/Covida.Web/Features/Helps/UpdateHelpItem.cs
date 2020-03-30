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
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Covida.Web.Features.Helps
{
    public class UpdateHelpItem
    {
        public class Command : IRequest<Unit>
        {
            public Guid HelpItemId { get; set; }
            public bool Complete { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.HelpItemId).NotNull();
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
                // Get help item
                var helpItem = await db.HelpItems
                    .Include(x => x.Help)
                    .FirstOrDefaultAsync(x => x.Id == request.HelpItemId);
                if(helpItem is null)
                {
                    throw new HttpException(HttpStatusCode.BadRequest, ErrorMessages.EntityNotFound<HelpItem>());
                }
                // Update its value
                helpItem.Complete = request.Complete;
                db.HelpItems.Update(helpItem);
                await db.SaveChangesAsync();
                // Notify the other person
                var hubClient = hubContext.Clients.User(helpItem.Help.AuthorId.ToString());
                if(hubClient != null)
                {
                    await hubClient.SendCoreAsync("updateHelpItem", new object[] { helpItem.Id, helpItem.Complete }, cancellationToken);
                }
                // Return result
                return Unit.Value;
            }
        }

    }

}
