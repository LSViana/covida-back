using Covida.Data.Postgre;
using Covida.Data.Scaffold;
using Covida.Web.Configuration;
using Covida.Web.Hubs;
using Covida.Web.Mediator;
using Covida.Web.Middlewares.HttpExceptionHandling;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using System.Text.Json.Serialization;

namespace Covida.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostEnvironment environment)
        {
            Configuration = configuration;
            HostEnvironment = environment;
        }

        public IConfiguration Configuration { get; }
        public IHostEnvironment HostEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddControllers()
                .AddJsonOptions(jsonOptions =>
                {
                    jsonOptions.JsonSerializerOptions.IgnoreNullValues = true;
                    jsonOptions.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                })
                .AddFluentValidation();

            // Registering all the validators for FluentValidation
            AssemblyScanner.FindValidatorsInAssembly(Assembly.GetExecutingAssembly())
                .ForEach(result => services.AddScoped(result.InterfaceType, result.ValidatorType));

            services.AddSwagger();
            services.AddCors();
            if (HostEnvironment.IsProduction())
            {
                services.AddPostgreProduction(Configuration);
                services.AddScoped<DbSeeder<CovidaDbContext>, ProductionSeeder>();
            }
            else if (HostEnvironment.IsDevelopment())
            {
                services.AddPostgreProduction(Configuration);
                //services.AddPostgreDevelopment(Configuration);
                services.AddScoped<DbSeeder<CovidaDbContext>, DevelopmentSeeder>();
            }
            services.AddSignalR();
            services.AddSignalRCore();
            services.AddSingleton<IUserIdProvider, IdUserIdProvider>();
            services.AddSingleton<HttpExceptionMiddleware>();
            services.AddJwtAuthentication(Configuration, HostEnvironment);
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ActorFetchHandler<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationHandler<,>));
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

            app.UseHttpExceptionHandler();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Covida V1");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<MessagesHub>(MessagesHub.Url);
                endpoints.MapControllers();
            });
        }
    }
}
