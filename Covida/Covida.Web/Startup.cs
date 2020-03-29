using Covida.Data.Postgre;
using Covida.Data.Scaffold;
using Covida.Web.Configuration;
using Covida.Web.Hubs;
using Covida.Web.Mediator;
using Covida.Web.Middlewares;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace Covida.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddControllers()
                .AddJsonOptions(jsonOptions =>
                {
                    jsonOptions.JsonSerializerOptions.IgnoreNullValues = true;
                })
                .AddFluentValidation();

            // Registering all the validators for FluentValidation
            AssemblyScanner.FindValidatorsInAssembly(Assembly.GetExecutingAssembly())
                .ForEach(result => services.AddScoped(result.InterfaceType, result.ValidatorType));

            services.AddCors();
            if (Environment.IsProduction())
            {
                services.AddPostgre(Configuration);
            }
            else if (Environment.IsDevelopment())
            {
                services.AddDbContext<CovidaDbContext>(x =>
                {
                    x.UseInMemoryDatabase("db");
                });
            }
            if (Environment.IsProduction())
            {
                services.AddSingleton<DbSeeder<CovidaDbContext>, DevelopmentSeeder>();
            }
            else
            {
                services.AddSingleton<DbSeeder<CovidaDbContext>, DevelopmentSeeder>();
            }
            services.AddSingleton<HttpExceptionMiddleware>();
            services.AddJwtAuthentication(Configuration);
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ActorFetchHandler<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationHandler<,>));
            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(builder =>
            {
                builder
                    .WithOrigins(Configuration.GetSection("Cors:OriginsAllowed").Get<string[]>())
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(routeBuilder =>
            {
                routeBuilder.MapHub<MessagesHub>(MessagesHub.Url);
            });

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Eventually V1");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
