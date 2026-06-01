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
    public async Task<IReadOnlyList<TEntity>?> GetAllAsync(CancellationToken ct, bool noTrackingEnabled = false, bool softDeletedItemsEnabled = false)
    {
        var query = _entityDbSet.AsQueryable();

        if (noTrackingEnabled)
            query = _entityDbSet.AsNoTracking();

        if (softDeletedItemsEnabled)
            query = _entityDbSet.IgnoreQueryFilters();

        return await query.ToListAsync(ct);
    }
    
    public async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken ct, bool noTrackingEnabled = false, bool softDeletedItemsEnabled = false)
    {
        var query = _entityDbSet.AsQueryable();

        if (noTrackingEnabled)
            query = _entityDbSet.AsNoTracking();

        if (softDeletedItemsEnabled)
            query = _entityDbSet.IgnoreQueryFilters();

        return await query.FirstOrDefaultAsync(e => e.Id == id, ct);
    }    

    public void Add(TEntity entity)
        => _entityDbSet.Add(entity);    

    public void SoftDelete(TEntity entity)
        => _entityDbSet.Remove(entity);    

    public void Update(TEntity entity)
        => _entityDbSet.Update(entity);    

    public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct, bool TrackingEnabled = false)
        => !TrackingEnabled ? await _entityDbSet.AsNoTracking().FirstOrDefaultAsync(predicate,ct) : await _entityDbSet.FirstOrDefaultAsync(predicate, ct);
   
    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct)
        => await _entityDbSet.AnyAsync(predicate,ct);

    public async Task<int> SaveChangesAsync(CancellationToken ct)
        => await _gymDbContext.SaveChangesAsync(ct);
}
