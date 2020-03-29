using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Covida.Web.Configuration
{
    public static class JwtAuthentication
    {
        public const string JwtBaseKey = "JwtOptions";

        public static void AddJwtAuthentication(this IServiceCollection @this, IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            var jwtOptions = configuration.GetSection(JwtBaseKey);
            @this
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = hostEnvironment.IsProduction();
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidAudience = jwtOptions["Audience"],
                        ValidIssuer = jwtOptions["Issuer"],
                        IssuerSigningKey = GetSecurityKey(jwtOptions["SecretKey"], jwtOptions["SecurityPhrase"]),
                        LifetimeValidator = (notBefore, expires, token, parameters) =>
                        {
                            var now = DateTime.UtcNow;
                            return (now >= notBefore) && (now <= expires);
                        },
                        //
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                    };
                    //
                    options.Validate();
                });
        }

        public static SecurityKey GetSecurityKey(string secretKey, string securityPhrase)
        {
            var algorithm = new HMACSHA256
            {
                Key = Encoding.UTF8.GetBytes(secretKey)
            };
            var hash = algorithm.ComputeHash(Encoding.UTF8.GetBytes(securityPhrase));
            return new SymmetricSecurityKey(hash);
        }
    }
}
