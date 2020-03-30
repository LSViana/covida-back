using Covida.Core.Domain;
using Covida.Core.Domain.Constants;
using Covida.Data.Postgre;
using Covida.Infrastructure.Definitions;
using Covida.Infrastructure.Exceptions;
using Covida.Infrastructure.Geometry;
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
    public class Create
    {
        public class Command : IActorAwareRequest<Result>
        {
            public int[] Categories { get; set; }
            public HelpItemCreate[] Items { get; set; }
            public User Actor { get; set; }

            public class HelpItemCreate
            {
                public string Name { get; set; }
                public uint Amount { get; set; }
            }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Categories).NotEmpty();
                RuleFor(x => x.Items).NotEmpty().DependentRules(() =>
                {
                    RuleForEach(x => x.Items).ChildRules((y) =>
                    {
                        y.RuleFor(y => y.Name).NotEmpty().Length(2, 64);
                        y.RuleFor(y => y.Amount).GreaterThan((uint)0);
                    });
                });
            }
        }

        public class Result
        {
            public Guid Id { get; set; }
            public HelpStatus HelpStatus { get; set; }
            public HelpItemResult[] HelpItems { get; set; }

            public class HelpItemResult
            {
                public Guid Id { get; set; }
                public string Name { get; set; }
                public uint Amount { get; set; }
            }
        }

        public class WebSocketResult : Result
        {
            public UserResult User { get; set; }

            public class UserResult
            {
                public int Id { get; set; }
                public string Name { get; set; }
                public PointD Location { get; set; }
            }
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
                // Validate HelpCategories
                var helpCategories = await db.HelpCategories
                    .Where(x => request.Categories.Contains(x.Id))
                    .ToListAsync();
                if(helpCategories.Count != request.Categories.Length)
                {
                    throw new HttpException(HttpStatusCode.BadRequest, ErrorMessages.EntityNotFound<HelpCategory>());
                }
                // Create the Help
                var help = new Help
                {
                    Id = Guid.NewGuid(),
                    CreatedAt = DateTime.Now,
                    HelpStatus = HelpStatus.Awaiting,
                    AuthorId = request.Actor.Id,
                    HelpItems = request.Items
                        .Select(x => new HelpItem
                        {
                            Name = x.Name,
                            Amount = x.Amount,
                            CreatedAt = DateTime.Now,
                            Complete = false,
                        })
                        .ToList(),
                    HelpHasCategories = helpCategories
                        .Select(x => new HelpHasCategory
                        {
                            HelpCategory = x,
                            CreatedAt = DateTime.Now,
                        })
                        .ToList(),
                };
                // Save to database
                await db.Helps.AddAsync(help);
                await db.SaveChangesAsync();
                var result = new Result
                {
                    Id = help.Id,
                    HelpStatus = help.HelpStatus,
                    HelpItems = help.HelpItems
                        .Select(x => new Result.HelpItemResult
                        {
                            Id = x.Id,
                            Name = x.Name,
                            Amount = x.Amount,
                        })
                        .ToArray(),
                };
                // Send the message for everyone in WebSocket
                await hubContext.Clients.Group(MessagesHub.VolunteerGroupName).SendCoreAsync("newHelp", new[] {
                    new WebSocketResult {
                        Id = result.Id,
                        HelpItems = result.HelpItems,
                        HelpStatus = result.HelpStatus,
                        User = new WebSocketResult.UserResult {
                            Id = help.Author.Id,
                            Name = help.Author.Name,
                            Location = help.Author.Location.ToPointD()
                        },
                    },
                }, cancellationToken);
                // Return the newly created Help mapped to result
                return result;
            }
        }

    }

}
