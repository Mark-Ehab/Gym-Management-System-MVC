using GymManagementSystem.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace GymManagementSystem.DataAccess.Repositories.Contracts;

public interface IGenericRepository<TEntity> where TEntity : BaseEntity, new()
{
    Task<IReadOnlyList<TEntity>?> GetAllAsync(CancellationToken ct , bool noTrackingEnabled = false, bool softDeletedItemsEnabled = false);
    Task<TEntity?> GetByIdAsync(Guid id, CancellationToken ct, bool noTrackingEnabled = false, bool softDeletedItemsEnabled = false);
    void Add(TEntity entity);
    void Update(TEntity entity);
    void SoftDelete(TEntity entity);
    Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity,bool>> predicate, CancellationToken ct, bool TrackingEnabled = false);
    Task<bool> AnyAsync(Expression<Func<TEntity,bool>> predicate, CancellationToken ct);
    Task<int> SaveChangesAsync(CancellationToken ct);
}
