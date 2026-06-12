using GymManagementSystem.DataAccess.Models;
using GymManagementSystem.DataAccess.Specifiction.Contract;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace GymManagementSystem.DataAccess.Repositories.Contracts;

public interface IGenericRepository<TEntity> where TEntity : BaseEntity, new()
{
    Task<IReadOnlyList<TEntity>?> GetAllAsync(bool noTrackingEnabled = false, bool softDeletedItemsEnabled = false, CancellationToken ct = default);
    Task<TEntity?> GetByIdAsync(Guid id,bool noTrackingEnabled = false, bool softDeletedItemsEnabled = false, CancellationToken ct = default);
    Task<TEntity?> FirstOrDefaultAsync(ISpecificaion<TEntity>? specificaion = null, bool TrackingEnabled = false, CancellationToken ct = default);
    Task<bool> AnyAsync(ISpecificaion<TEntity>? specificaion = null, CancellationToken ct = default);
    Task<int> CountAsync(ISpecificaion<TEntity>? specificaion = null, CancellationToken ct = default);
    Task<IReadOnlyList<TEntity>> ListAsync(ISpecificaion<TEntity> specificaion, bool TrackingEnabled = false, CancellationToken ct = default);
    Task<int> SaveChangesAsync(CancellationToken ct);
    void Add(TEntity entity);
    void Update(TEntity entity);
    void SoftDelete(TEntity entity);
}
