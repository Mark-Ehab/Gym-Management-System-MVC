using GymManagementSystem.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.DataAccess.Interceptors;

public sealed class AuditColumnsInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        /* Check if current DbContext is not null */
        var context = eventData.Context;
        if (context is null)
            return new(result);

        /* Update audit columns for tracked entites extending BaseEntity whose state is either Added or Modified */
        this.UpdateAuditColumns(context);

        return new(result);
    }

    private void UpdateAuditColumns(DbContext context)
    {
        var entries = context.ChangeTracker.Entries();

        foreach (var entry in entries)
        {
            if (entry is null || entry.Entity is not BaseEntity || entry.State is not (EntityState.Added or EntityState.Modified))
                continue;

            switch (entry.State)
            { 
                case EntityState.Added:
                    entry.Property(nameof(BaseEntity.CreatedAt)).CurrentValue = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Property(nameof(BaseEntity.ModifiedAt)).CurrentValue = DateTime.UtcNow;
                    break;
            }
        }
    }
}
