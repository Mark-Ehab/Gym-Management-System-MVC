using GymManagementSystem.DataAccess.Models;
using GymManagementSystem.DataAccess.Specifiction.Contract;
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
    Task<TEntity?> FirstOrDefaultAsync(CancellationToken ct, ISpecificaion<TEntity>? specificaion = null, bool TrackingEnabled = false);
    Task<bool> AnyAsync(CancellationToken ct, ISpecificaion<TEntity>? specificaion = null);
    Task<IEnumerable<TEntity>> ListAsync(ISpecificaion<TEntity> specificaion, CancellationToken ct, bool TrackingEnabled = false);
    Task<int> SaveChangesAsync(CancellationToken ct);
}
