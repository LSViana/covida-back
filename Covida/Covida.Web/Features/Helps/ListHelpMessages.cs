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

namespace Covida.Web.Features.Helps
{
    public class ListHelpMessages
    {
        public class Query : PageRequest<Result>
        {
            public Guid HelpId { get; set; }
        }

        public class CommandValidator : AbstractValidator<Query>
        {
            public CommandValidator()
            {
                RuleFor(x => x.HelpId).NotEmpty();
            }
        }

        public class Result
        {
            public Guid Id { get; set; }
            public string AuthorName { get; set; }
            public string Text { get; set; }
        }

        public class RequestHandler : IRequestHandler<Query, PageResult<Result>>
        {
            private readonly CovidaDbContext db;

            public RequestHandler(CovidaDbContext db)
            {
                this.db = db;
            }

            public async Task<PageResult<Result>> Handle(Query request, CancellationToken cancellationToken)
            {
                // Get help and verify if it exists
                var help = await db.Helps
                    .Where(x => x.HelpStatus == Core.Domain.Constants.HelpStatus.Active)
                    .FirstOrDefaultAsync(x => x.Id == request.HelpId);
                if (help is null)
                {
                    throw new HttpException(HttpStatusCode.BadRequest, ErrorMessages.EntityNotFound<Help>());
                }
                // Return messages
                return await request.GetResult(db.Messages
                    .Where(x => x.HelpId == help.Id)
                    .OrderByDescending(x => x.CreatedAt)
                    .Select(x => new Result
                    {
                        Id = x.Id,
                        AuthorName = x.User.Name,
                        Text = x.Text,
                    })
                );
            }
        }

    }

}
