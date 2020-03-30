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
    public class SendMessageChat
    {
        public class Command : IActorAwareRequest<Result>
        {
            public Guid HelpId { get; set; }
            public string Text { get; set; }
            public User Actor { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.HelpId).NotEmpty();
                RuleFor(x => x.Text).NotEmpty().MaximumLength(256);
            }
        }

        public class Result
        {
            public Guid Id { get; set; }
        }

        public class RequestHandler : IRequestHandler<Command, Result>
        {
            private readonly CovidaDbContext db;
            private readonly IHubContext<MessagesHub> hubContext;

            public RequestHandler(CovidaDbContext db, IHubContext<MessagesHub> hubContext)
            {
                this.db = db;
                this.hubContext = hubContext;
            }

            public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                // Get help and verify if it exists
                var help = await db.Helps
                    .Where(x => x.HelpStatus == Core.Domain.Constants.HelpStatus.Active)
                    .FirstOrDefaultAsync(x => x.Id == request.HelpId);
                if (help is null)
                {
                    throw new HttpException(HttpStatusCode.BadRequest, ErrorMessages.EntityNotFound<Help>());
                }
                // Create message
                var message = new Message
                {
                    Id = Guid.NewGuid(),
                    CreatedAt = DateTime.Now,
                    MessageStatus = Core.Domain.Constants.MessageStatus.Sent,
                    Text = request.Text,
                    Help = help,
                    User = request.Actor,
                };
                // Save it to database
                await db.Messages.AddAsync(message);
                await db.SaveChangesAsync();
                // Send message via WebSocket for people in the HelpId group
                var currentUser = hubContext.Clients.User(request.Actor.Id.ToString());
                await hubContext.Clients.Group(help.Id.ToString()).SendCoreAsync("message", new object[] { request.Actor.Id, message.Id, request.Actor.Name, message.Text }, cancellationToken);
                // Return result
                return new Result
                {
                    Id = message.Id,
                };
            }
        }

    }

}
