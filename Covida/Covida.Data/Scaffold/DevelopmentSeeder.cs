using Covida.Core.Domain;
using Covida.Data.Postgre;
using Microsoft.EntityFrameworkCore;
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

        protected override async Task ClearDatabase()
        {
            DbContext.HelpHasCategories.RemoveRange(await DbContext.HelpHasCategories.IgnoreQueryFilters().ToListAsync());
            DbContext.HelpItems.RemoveRange(await DbContext.HelpItems.IgnoreQueryFilters().ToListAsync());
            DbContext.Messages.RemoveRange(await DbContext.Messages.ToListAsync());
            DbContext.HelpCategories.RemoveRange(await DbContext.HelpCategories.IgnoreQueryFilters().ToListAsync());
            DbContext.Helps.RemoveRange(await DbContext.Helps.IgnoreQueryFilters().ToListAsync());
            DbContext.Users.RemoveRange(await DbContext.Users.IgnoreQueryFilters().ToListAsync());
            await DbContext.SaveChangesAsync();
        }

        public override async Task Run()
        {
            await ClearDatabase();
            await AddUsers();
            await AddHelpCategories();
            await AddHelps();
        }

        private async Task AddHelps()
        {

            helps.Add(new Help
            {
                Id = Guid.Parse("{A9612A5A-F2E1-45A6-AFEB-C68DCD8238D0}"),
                HelpStatus = Core.Domain.Constants.HelpStatus.Awaiting,
                Author = users.ElementAt(0),
                HelpItems = new[]
                {
                        new HelpItem {
                            Id = Guid.Parse("{A9612A5A-F2E1-45A6-AFEB-C68DCD8238D0}"),
                            Name = "Apple",
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
                HelpStatus = Core.Domain.Constants.HelpStatus.Active,
                Author = users.ElementAt(1),
                HelpItems = new[]
                {
                    new HelpItem {
                        Id = Guid.Parse("{A9612A5A-F2E1-45A6-AFEB-C68DCD8238D8}"),
                        Name = "Bread",
                        Amount = 2,
                        Complete = false,
                        CreatedAt = DateTime.Now,
                    },
                },
                HelpHasCategories = new[]
                {
                    new HelpHasCategory { HelpCategory = helpCategories.ElementAt(1) }
                },
                Volunteer = users.ElementAt(0),
                CreatedAt = DateTime.Now,
            });
            helps.Add(new Help
            {
                Id = Guid.Parse("{A9612A5A-F2E1-45A6-AFEB-C68DCD8238D2}"),
                HelpStatus = Core.Domain.Constants.HelpStatus.Cancelled,
                Author = users.ElementAt(1),
                HelpItems = new[]
                {
                    new HelpItem {
                        Id = Guid.Parse("{A9612A5A-F2E1-45A6-AFEB-C68DCD8238D9}"),
                        Name = "Bread",
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
                Name = "Market",
            });
            helpCategories.Add(new HelpCategory
            {
                Id = 2,
                Name = "Bakery",
            });
            helpCategories.Add(new HelpCategory
            {
                Id = 3,
                Name = "Pharmacy",
            });
            helpCategories.Add(new HelpCategory
            {
                Id = 4,
                Name = "Other",
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
                Latitude = -23.5646143,
                Longitude = -46.4159218,
                IsVolunteer = true,
                CreatedAt = DateTime.Now,
                DeletedAt = null,
            });
            users.Add(new User
            {
                Id = 2,
                Name = "Gustavo Henrique",
                Address = "Al. Barão de Limeira, 549",
                Latitude = -23.5646143,
                Longitude = -46.4159218,
                IsVolunteer = false,
                CreatedAt = DateTime.Now,
                DeletedAt = null,
            });
            users.Add(new User
            {
                Id = 3,
                Name = "Lucas Souza",
                Address = "Al. Barão de Limeira, 549",
                Latitude = -23.5646143,
                Longitude = -46.4159218,
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
