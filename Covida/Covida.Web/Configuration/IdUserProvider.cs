using Covida.Core.Domain;
using Covida.Infrastructure.Definitions;
using Covida.Infrastructure.Exceptions;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Covida.Web.Configuration
{
    public class IdUserIdProvider : IUserIdProvider
    {
        public IdUserIdProvider()
        {
        }

        public string GetUserId(HubConnectionContext connection)
        {
            var userId = connection.User?.FindFirst(nameof(User.Id))?.Value;
            if (userId is null)
            {
                throw new HttpException(HttpStatusCode.BadRequest, ErrorMessages.EntityNotFound<User>());
            }
            else
            {
                return userId;
            }
        }
    }
}
