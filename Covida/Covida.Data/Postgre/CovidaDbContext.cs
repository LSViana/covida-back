using Covida.Core.Definition;
using Covida.Core.Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Covida.Data.Postgre
{
    public class CovidaDbContext : DbContext
    {
        /// <summary>
        /// This constructor is required for Entity Framework to pass options into this <see cref="DbContext"/>.
        /// </summary>
        /// <param name="options">Options to define the behavior of this <see cref="DbContext"/>.</param>
        public CovidaDbContext(DbContextOptions<CovidaDbContext> options) : base(options)
        {
            // Nothing to do here.
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Ensuring database has PostGIS installed
            modelBuilder.HasPostgresExtension("postgis");
            // Adding rules for all entitites
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                #region Adding query filters to avoid returning deleted entities
                // Get the DeletedAt property info
                var isDeletedProperty = entityType.FindProperty(nameof(IDomain.DeletedAt));
                // Create a parameter that represents the instance when applying filters (where in SQL)
                ParameterExpression entityParameter = Expression.Parameter(entityType.ClrType, "x");
                // Creating the LambdaExpression to filter this entity and return only not deleted records
                var isDeletedFilter = Expression.Lambda(Expression.Equal(Expression.Property(entityParameter, isDeletedProperty.PropertyInfo), Expression.Constant(null)), entityParameter);
                // Applying the query filter to be executed in every SELECT * FROM <tableName> for this entity
                entityType.SetQueryFilter(isDeletedFilter);
                #endregion
                #region Removing delete policies to avoid errors with relational constraints
                foreach (var navigation in entityType.GetNavigations())
                {
                    navigation.ForeignKey.DeleteBehavior = DeleteBehavior.Restrict;
                }
                #endregion
            }
            #region Compound Primary Keys
            modelBuilder.Entity<HelpHasCategory>()
                .HasKey(x => new
                {
                    x.HelpId,
                    x.HelpCategoryId,
                });
            #endregion
            #region Help multiple relations to User
            modelBuilder.Entity<Help>()
                .HasOne(x => x.Author)
                .WithMany(x => x.Helps)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Help>()
                .HasOne(x => x.Volunteer)
                .WithMany(x => x.Answers)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion
        }

        #region Database sets
        public DbSet<User> Users { get; set; }
        public DbSet<HelpCategory> HelpCategories { get; set; }
        public DbSet<Help> Helps { get; set; }
        public DbSet<HelpHasCategory> HelpHasCategories { get; set; }
        public DbSet<HelpItem> HelpItems { get; set; }
        public DbSet<Message> Messages { get; set; }
        #endregion
    }
}
