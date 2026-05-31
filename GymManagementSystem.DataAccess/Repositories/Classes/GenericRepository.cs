using GymManagementSystem.DataAccess.Data.Contexts;
using GymManagementSystem.DataAccess.Models;
using GymManagementSystem.DataAccess.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace GymManagementSystem.DataAccess.Repositories.Classes;

public sealed class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity, new()
{
    /* Fields */
    private readonly GymDbContext _gymDbContext;
    private readonly DbSet<TEntity> _entityDbSet;

    /* Constructor */
    public GenericRepository(GymDbContext context)
    {
        _gymDbContext = context;
        _entityDbSet = _gymDbContext.Set<TEntity>();
    }

    /* Methods */
    public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken ct, bool TrackingEnabled = false)
        => !TrackingEnabled ? await _entityDbSet.AsNoTracking().ToListAsync(ct) : await _entityDbSet.ToListAsync(ct);
    

    public async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken ct, bool TrackingEnabled = false)
        => !TrackingEnabled ? await _entityDbSet.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id,ct) : await _entityDbSet.FindAsync(id,ct);
    

    public async Task<int> AddAsync(TEntity entity, CancellationToken ct)
    {
        _entityDbSet.Add(entity);

        return await _gymDbContext.SaveChangesAsync(ct);
    }

    public async Task<int> DeleteAsync(TEntity entity, CancellationToken ct)
    {
        _entityDbSet.Remove(entity);

        return await _gymDbContext.SaveChangesAsync(ct);
    }

    public async Task<int> UpdateAsync(TEntity entity, CancellationToken ct)
    {
        _entityDbSet.Update(entity);

        return await _gymDbContext.SaveChangesAsync(ct);
    }

    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct)
        => await _entityDbSet.AnyAsync(predicate,ct);

    public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct, bool TrackingEnabled = false)
        => !TrackingEnabled ? await _entityDbSet.AsNoTracking().FirstOrDefaultAsync(predicate,ct) : await _entityDbSet.FirstOrDefaultAsync(predicate, ct);
}
