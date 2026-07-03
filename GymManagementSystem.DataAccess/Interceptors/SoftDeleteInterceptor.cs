using GymManagementSystem.DataAccess.Models.BusinessModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.DataAccess.Interceptors;

public sealed class SoftDeleteInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        /* Check if current DbContext is not null */
        var context = eventData.Context;
        if (context is null)
            return new(result);

        /* Soft delete tracked entites extending BaseEnitiy whose State is Deleted */
        this.ApplySoftDelete(context);

        return new(result);
    }

    private void ApplySoftDelete(DbContext context)
    {
        var entries = context.ChangeTracker.Entries();

        foreach (var entry in entries)
        {
            if (entry is null ||
               entry.Entity is not BaseEntity entity ||
               entry.State is not EntityState.Deleted ||
               entity.IsDeleted is true)
                continue;
            entry.State = EntityState.Modified;
            entity.IsDeleted = true;
            entity.DeletedAt = DateTime.Now;

            foreach(var ReferenceEntry in entry.References)
            {
                if(ReferenceEntry.TargetEntry is not null && 
                    ReferenceEntry.TargetEntry.Metadata.IsOwned() &&
                    ReferenceEntry.TargetEntry.State == EntityState.Deleted)
                {
                    ReferenceEntry.TargetEntry.State = EntityState.Modified;
                }
            }
        }
    }
}
