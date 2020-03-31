using Covida.Data.Postgre;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Covida.Web.Configuration
{
    public static class PostgreConnection
    {
        public static void AddPostgre(this IServiceCollection @this, IConfiguration configuration)
        {
            var key = "Postgre";
            // Registering the database context
            @this.AddDbContext<CovidaDbContext>(builder =>
            {
                // Adding PostgreSQL configuration
                builder.UseNpgsql(
                    configuration.GetConnectionString(key),
                    npgOptions =>
                    {
                        npgOptions.MigrationsAssembly($"{nameof(Covida)}.{nameof(Web)}");
                    });
            }, ServiceLifetime.Transient);
        }
    }
}
