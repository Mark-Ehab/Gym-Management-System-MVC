using GymManagementSystem.DataAccess.Data.Configurations;
using GymManagementSystem.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace GymManagementSystem.DataAccess.Data.Contexts;

public sealed class GymDbContext : DbContext
{
    /* Constructor */
    public GymDbContext(DbContextOptions<GymDbContext> options) : base(options) { }
    
    /* DbSets */
    public DbSet<Member> Members { get; set; }
    public DbSet<HealthRecord> HealthRecords { get; set; }
    public DbSet<Plan> Plans { get; set; }
    public DbSet<Trainer> Trainers { get; set; }
    public DbSet<Session> Sessions { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<Membership> Memberships { get; set; }

    /* Methods */
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        /* Apply all models configurations built through Fluent API */
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        var modelEntityTypes = modelBuilder.Model.GetEntityTypes();
        foreach (var modelEntityType in modelEntityTypes)
        {
            /* Check if model is of type BaseEntity */
            if (!typeof(BaseEntity).IsAssignableFrom(modelEntityType.ClrType))
                continue;

            /* Apply a query filter for all models that extends BaseEntity to retrieve only items that are not soft deleted */
            var param = Expression.Parameter(modelEntityType.ClrType, "e");
            var body = Expression.Not(Expression.Property(param,nameof(BaseEntity.IsDeleted)));
            var lambdaExpression = Expression.Lambda(body, param);
            modelEntityType.SetQueryFilter(lambdaExpression);

            /* Set Id property in all models that extends BaseEntity as the primary key */
            modelEntityType.SetPrimaryKey(modelEntityType.FindProperty(nameof(BaseEntity.Id)));
            modelEntityType.FindProperty(nameof(BaseEntity.Id))
                !.SetDefaultValueSql("NEWSEQUENTIALID()");
            modelEntityType.FindProperty(nameof(BaseEntity.Id))
                !.SetColumnOrder(1);

            /* Set IsDeleted property default value in all models that extends BaseEntity as false */
            modelEntityType.FindProperty(nameof(BaseEntity.IsDeleted))
                !.SetDefaultValue(false);
        }
    }
}
