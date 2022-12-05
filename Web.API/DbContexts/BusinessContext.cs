using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using Web.API.Bases;
using Web.API.Models;

namespace Web.API.DbContexts
{
    public class BusinessContext : DbContext
    {
        public BusinessContext(DbContextOptions options)
       : base(options)
        {
            
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<RoleGroup> RoleGroups { get; set; }
        public DbSet<Role> Roles { get; set; }

        /// <summary>
        /// Filters all entity types while querying, which are derived from EntityBase
        /// </summary>
        /// <param name="modelBuilder"></param>
        public void IsActiveFilter(ModelBuilder modelBuilder)
        {
            Expression<Func<EntityBase, bool>> filterExpr = bm => bm.IsActive;
            foreach (var entityType in modelBuilder.Model.GetEntityTypes().Where(entity => entity.ClrType.IsAssignableTo(typeof(EntityBase))))
            {
                var parameter = Expression.Parameter(entityType.ClrType);
                var body = ReplacingExpressionVisitor.Replace(filterExpr.Parameters.First(), parameter, filterExpr.Body);
                var lambdaExpression = Expression.Lambda(body, parameter);
                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambdaExpression);
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            IsActiveFilter(modelBuilder);

            #region Employee - RoleGroup

            modelBuilder.Entity<Employee>()
                .HasMany(p => p.RoleGroups)
                .WithMany(p => p.Employees)
                .UsingEntity<Dictionary<string, object>>(
                 j => j
                     .HasOne<RoleGroup>()
                     .WithMany()
                     .HasForeignKey("RoleGroupId")
                     .OnDelete(DeleteBehavior.Cascade),
                 j => j
                     .HasOne<Employee>()
                     .WithMany()
                     .HasForeignKey("EmployeeId")
                     .OnDelete(DeleteBehavior.Cascade));
            #endregion

            #region Product - Category
            modelBuilder.Entity<Product>()
                  .HasMany(p => p.Categories)
                  .WithMany(p => p.Products)
                  .UsingEntity<Dictionary<string, object>>(
                   j => j
                       .HasOne<Category>()
                       .WithMany()
                       .HasForeignKey("CategoryId")
                       .OnDelete(DeleteBehavior.Cascade),
                   j => j
                       .HasOne<Product>()
                       .WithMany()
                       .HasForeignKey("ProductId")
                       .OnDelete(DeleteBehavior.Cascade));
            #endregion

            #region Product - Company
            modelBuilder.Entity<Product>()
                  .HasMany(p => p.Companies)
                  .WithMany(p => p.Products)
                  .UsingEntity<Dictionary<string, object>>(
                   j => j
                       .HasOne<Company>()
                       .WithMany()
                       .HasForeignKey("CompanyId")
                       .OnDelete(DeleteBehavior.Cascade),
                   j => j
                       .HasOne<Product>()
                       .WithMany()
                       .HasForeignKey("ProductId")
                       .OnDelete(DeleteBehavior.Cascade));
            #endregion

            #region RoleGroup - Employee
            modelBuilder.Entity<RoleGroup>()
                  .HasMany(p => p.Employees)
                  .WithMany(p => p.RoleGroups)
                  .UsingEntity<Dictionary<string, object>>(
                   j => j
                       .HasOne<Employee>()
                       .WithMany()
                       .HasForeignKey("EmployeeId")
                       .OnDelete(DeleteBehavior.Cascade),
                   j => j
                       .HasOne<RoleGroup>()
                       .WithMany()
                       .HasForeignKey("RoleGroupId")
                       .OnDelete(DeleteBehavior.Cascade));
            #endregion

            #region RoleGroup - Roles
            modelBuilder.Entity<RoleGroup>()
                  .HasMany(p => p.Roles)
                  .WithMany(p => p.RoleGroups)
                  .UsingEntity<Dictionary<string, object>>(
                   j => j
                       .HasOne<Role>()
                       .WithMany()
                       .HasForeignKey("RoleId")
                       .OnDelete(DeleteBehavior.Cascade),
                   j => j
                       .HasOne<RoleGroup>()
                       .WithMany()
                       .HasForeignKey("RoleGroupId")
                       .OnDelete(DeleteBehavior.Cascade));
            #endregion

        }
    }
}
