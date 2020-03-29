using Covida.Core.Domain;
using Covida.Data.Postgre;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace Covida.Data.Scaffold
{
    public class DevelopmentSeeder : DbSeeder<CovidaDbContext>
    {
        public DevelopmentSeeder(CovidaDbContext dbContext) : base(dbContext)
        {
        }

        public override async Task Run()
        {
            await AddUsers();
            await AddHelpCategories();
        }

        private async Task AddHelpCategories()
        {
            await DbContext.HelpCategories.AddRangeAsync(new[]
            {
                new HelpCategory
                {
                    Id = Guid.Parse("{A9612A5A-F2E1-45A6-AFEB-C68DCD8238D0}"),
                    Name = "Mercado",
                },
                new HelpCategory
                {
                    Id = Guid.Parse("{A9612A5A-F2E1-45A6-AFEB-C68DCD8238D1}"),
                    Name = "Padaria",
                },
                new HelpCategory
                {
                    Id = Guid.Parse("{A9612A5A-F2E1-45A6-AFEB-C68DCD8238D2}"),
                    Name = "Farmácia",
                },
                new HelpCategory
                {
                    Id = Guid.Parse("{A9612A5A-F2E1-45A6-AFEB-C68DCD8238D3}"),
                    Name = "Outros",
                }
            });
        }

        private async Task AddUsers()
        {
            await DbContext.Users.AddRangeAsync(new[]
            {
                new User
                {
                    Id = 1,
                    Name = "Lucas Viana",
                    Address = "Al. Barão de Limeira, 539",
                    Location = new PointF(-23.5363939f, -46.6462365f),
                    IsVolunteer = true,
                    CreatedAt = new DateTime(),
                    DeletedAt = null,
                },
                new User
                {
                    Id = 2,
                    Name = "Gustavo Henrique",
                    Address = "Al. Barão de Limeira, 549",
                    Location = new PointF(-23.536638f,-46.645745f),
                    IsVolunteer = false,
                    CreatedAt = new DateTime(),
                    DeletedAt = null,
                },
                new User
                {
                    Id = 3,
                    Name = "Lucas Souza",
                    Address = "Al. Barão de Limeira, 549",
                    Location = new PointF(-23.536638f,-46.645745f),
                    IsVolunteer = false,
                    CreatedAt = new DateTime(),
                    DeletedAt = new DateTime(),
                }
            });
        }

        public override async Task Commit()
        {
            await DbContext.SaveChangesAsync();
        }
    }
}
