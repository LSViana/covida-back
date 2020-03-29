using Covida.Core.Definition;
using Covida.Core.Domain;
using Covida.Data.Postgre;
using Covida.Infrastructure.Definitions;
using Covida.Infrastructure.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Covida.Web.Mediator
{
    public class ActorFetchHandler<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IHttpContextAccessor contextAccessor;
        private readonly CovidaDbContext db;

        public ActorFetchHandler(IHttpContextAccessor contextAccessor, CovidaDbContext db)
        {
            this.contextAccessor = contextAccessor;
            this.db = db;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (request is IActorAwareRequest<TResponse> actorAwareRequest)
            {
                // If it's not null, keep the current value
                if (actorAwareRequest.Actor is null)
                {
                    var idClaim = contextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == nameof(User.Id));
                    if (idClaim is null)
                    {
                        throw new HttpException(HttpStatusCode.BadRequest, ErrorMessages.InvalidToken);
                    }
                    var id = Guid.Parse(idClaim.Value);
                    var user = await db.Users
                        .FirstOrDefaultAsync(x => x.Id == id);
                    if (user is null || user.IsDeleted())
                    {
                        throw new HttpException(HttpStatusCode.NotFound, ErrorMessages.EntityNotFound<User>());
                    }
                    actorAwareRequest.Actor = user;
                }
            }
            var response = await next();
            return response;
        }
    }
}
