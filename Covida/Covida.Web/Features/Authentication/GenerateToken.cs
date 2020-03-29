using Covida.Core.Domain;
using Covida.Data.Postgre;
using Covida.Web.Configuration;
using Microsoft.IdentityModel.Tokens;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Security.Claims;

namespace Covida.Web.Features.Authentication
{
    public class GenerateToken
    {
        public class Query : IRequest<Result>
        {
            public User User { get; set; }
        }

        public class Result
        {
            public string Token { get; set; }
            public DateTime ExpiresAt { get; set; }
            public int Id { get; set; }
        }

        public class RequestHandler : IRequestHandler<Query, Result>
        {
            private readonly IConfiguration configuration;

            public RequestHandler(IConfiguration configuration)
            {
                this.configuration = configuration;
            }

            public async Task<Result> Handle(Query request, CancellationToken cancellationToken)
            {
                // Create token handler, which is responsible of generating JWT
                var jwtHandler = new JwtSecurityTokenHandler();
                // Get JWT options from configuration file
                var jwtOptions = configuration.GetSection(JwtAuthentication.JwtBaseKey);
                var lifetimeDays = int.Parse(jwtOptions["LifetimeDays"]);
                // Token parameters
                var tokenLifetime = TimeSpan.FromDays(lifetimeDays);
                var now = DateTime.Now;
                // Create token
                var jwt = jwtHandler.CreateJwtSecurityToken(new SecurityTokenDescriptor
                {
                    Audience = jwtOptions["Audience"],
                    Issuer = jwtOptions["Issuer"],
                    Expires = now + tokenLifetime,
                    IssuedAt = now,
                    NotBefore = now,
                    Subject = new ClaimsIdentity(new[]
                    {
                        new Claim(nameof(User.Id), request.User.Id.ToString()),
                    }),
                    SigningCredentials =
                        new SigningCredentials(JwtAuthentication.GetSecurityKey(jwtOptions["SecretKey"], jwtOptions["SecurityPhrase"]), SecurityAlgorithms.HmacSha256),
                });
                // Returning created token
                return await Task.FromResult(new Result
                {
                    Id = request.User.Id,
                    Token = jwtHandler.WriteToken(jwt),
                    ExpiresAt = jwt.ValidTo,
                });
            }
        }
    }

}
