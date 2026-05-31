using GymManagementSystem.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace GymManagementSystem.DataAccess.Repositories.Contracts;

public interface IGenericRepository<TEntity> where TEntity : BaseEntity, new()
{
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken ct , bool TrackingEnabled = false);
    Task<TEntity?> GetByIdAsync(Guid id, CancellationToken ct, bool TrackingEnabled = false);
    Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity,bool>> predicate, CancellationToken ct, bool TrackingEnabled = false);
    Task<int> AddAsync(TEntity entity, CancellationToken ct);
    Task<int> UpdateAsync(TEntity entity, CancellationToken ct);
    Task<int> DeleteAsync(TEntity entity, CancellationToken ct);
    Task<bool> AnyAsync(Expression<Func<TEntity,bool>> predicate, CancellationToken ct);
}
