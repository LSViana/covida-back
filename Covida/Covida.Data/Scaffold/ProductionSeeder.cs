using Covida.Core.Domain;
using Covida.Data.Postgre;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace Covida.Data.Scaffold
{
    public class ProductionSeeder : DbSeeder<CovidaDbContext>
    {
        public ProductionSeeder(CovidaDbContext dbContext) : base(dbContext)
        {
        }

        public override async Task Run()
        {
            // Production seeder has no data yet.
            await Task.CompletedTask;
        }

        public override async Task Commit()
        {
            await DbContext.SaveChangesAsync();
        }
    }
}
