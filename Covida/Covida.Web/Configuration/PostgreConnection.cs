using Covida.Data.Postgre;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

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
                    // The following configuration is needed to keep using DbContext out of the Eventually.Web project
                    npgOptions =>
                    {
                        // Adding NetTopologySuite support everywhere
                        npgOptions.UseNetTopologySuite();
                        npgOptions.MigrationsAssembly($"{nameof(Covida)}.{nameof(Web)}");
                    });
            });
        }
    }
}
