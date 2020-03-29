using Covida.Core.Domain;
using Covida.Data.Postgre;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Covida.Data.Scaffold
{
    public class DevelopmentSeeder : DbSeeder<CovidaDbContext>
    {
        private ICollection<User> users;
        private ICollection<HelpCategory> helpCategories;
        private ICollection<Help> helps;

        public DevelopmentSeeder(CovidaDbContext dbContext) : base(dbContext)
        {
            users = new List<User>();
            helpCategories = new List<HelpCategory>();
            helps = new List<Help>();
        }

        public override async Task Run()
        {
            await AddUsers();
            await AddHelpCategories();
            await AddHelps();
        }

        private async Task AddHelps()
        {

            helps.Add(new Help
            {
                Id = Guid.Parse("{A9612A5A-F2E1-45A6-AFEB-C68DCD8238D0}"),
                HelpStatus = Core.Domain.Constants.HelpStatus.Active,
                User = users.ElementAt(0),
                HelpItems = new[]
                {
                        new HelpItem {
                            Id = Guid.Parse("{A9612A5A-F2E1-45A6-AFEB-C68DCD8238D0}"),
                            Name = "Maçã",
                            Amount = 2,
                            Complete = false,
                            CreatedAt = DateTime.Now,
                        },
                        new HelpItem {
                            Id = Guid.Parse("{A9612A5A-F2E1-45A6-AFEB-C68DCD8238D1}"),
                            Name = "Banana",
                            Amount = 4,
                            Complete = false,
                            CreatedAt = DateTime.Now,
                        },
                    },
                HelpHasCategories = new[]
                {
                    new HelpHasCategory { HelpCategory = helpCategories.ElementAt(0) }
                },
                CreatedAt = DateTime.Now,
            });
            helps.Add(new Help
            {
                Id = Guid.Parse("{A9612A5A-F2E1-45A6-AFEB-C68DCD8238D1}"),
                HelpStatus = Core.Domain.Constants.HelpStatus.Awaiting,
                User = users.ElementAt(1),
                HelpItems = new[]
                {
                    new HelpItem {
                        Id = Guid.Parse("{A9612A5A-F2E1-45A6-AFEB-C68DCD8238D8}"),
                        Name = "Pão",
                        Amount = 2,
                        Complete = false,
                        CreatedAt = DateTime.Now,
                    },
                },
                HelpHasCategories = new[]
                {
                    new HelpHasCategory { HelpCategory = helpCategories.ElementAt(1) }
                },
                CreatedAt = DateTime.Now,
            });
            helps.Add(new Help
            {
                Id = Guid.Parse("{A9612A5A-F2E1-45A6-AFEB-C68DCD8238D2}"),
                HelpStatus = Core.Domain.Constants.HelpStatus.Cancelled,
                User = users.ElementAt(1),
                HelpItems = new[]
                {
                    new HelpItem {
                        Id = Guid.Parse("{A9612A5A-F2E1-45A6-AFEB-C68DCD8238D9}"),
                        Name = "Pão",
                        Amount = 6,
                        Complete = false,
                        CreatedAt = DateTime.Now,
                    },
                },
                HelpHasCategories = new[]
                {
                    new HelpHasCategory { HelpCategory = helpCategories.ElementAt(1) }
                },
                CreatedAt = DateTime.Now,
            });
            await DbContext.Helps.AddRangeAsync(helps);
        }

        private async Task AddHelpCategories()
        {
            helpCategories.Add(new HelpCategory
            {
                Id = 1,
                Name = "Mercado",
            });
            helpCategories.Add(new HelpCategory
            {
                Id = 2,
                Name = "Padaria",
            });
            helpCategories.Add(new HelpCategory
            {
                Id = 3,
                Name = "Farmácia",
            });
            helpCategories.Add(new HelpCategory
            {
                Id = 4,
                Name = "Outros",
            });
            await DbContext.HelpCategories.AddRangeAsync(helpCategories);
        }

        private async Task AddUsers()
        {
            users.Add(new User
            {
                Id = 1,
                Name = "Lucas Viana",
                Address = "Al. Barão de Limeira, 539",
                Location = new Point(-23.5363939f, -46.6462365f) { SRID = Core.Domain.Constants.Location.DefaultSRID },
                IsVolunteer = true,
                CreatedAt = DateTime.Now,
                DeletedAt = null,
            });
            users.Add(new User
            {
                Id = 2,
                Name = "Gustavo Henrique",
                Address = "Al. Barão de Limeira, 549",
                Location = new Point(-23.536638f, -46.645745f) { SRID = Core.Domain.Constants.Location.DefaultSRID },
                IsVolunteer = false,
                CreatedAt = DateTime.Now,
                DeletedAt = null,
            });
            users.Add(new User
            {
                Id = 3,
                Name = "Lucas Souza",
                Address = "Al. Barão de Limeira, 549",
                Location = new Point(-23.536638f, -46.645745f) { SRID = Core.Domain.Constants.Location.DefaultSRID },
                IsVolunteer = false,
                CreatedAt = DateTime.Now,
                DeletedAt = DateTime.Now,
            });
            await DbContext.Users.AddRangeAsync(users);
        }

        public override async Task Commit()
        {
            await DbContext.SaveChangesAsync();
        }
    }
}
