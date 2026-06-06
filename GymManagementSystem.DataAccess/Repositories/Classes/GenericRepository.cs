using GymManagementSystem.DataAccess.Data.Contexts;
using GymManagementSystem.DataAccess.Models;
using GymManagementSystem.DataAccess.Repositories.Contracts;
using GymManagementSystem.DataAccess.Specifiction.Classes;
using GymManagementSystem.DataAccess.Specifiction.Contract;
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
            query = query.AsNoTracking();

        if (softDeletedItemsEnabled)
            query = query.IgnoreQueryFilters();

        return await query.ToListAsync(ct);
    }
    
    public async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken ct, bool noTrackingEnabled = false, bool softDeletedItemsEnabled = false)
    {
        var query = _entityDbSet.AsQueryable();

        if (noTrackingEnabled)
            query = query.AsNoTracking();

        if (softDeletedItemsEnabled)
            query = query.IgnoreQueryFilters();

        return await query.FirstOrDefaultAsync(e => e.Id == id, ct);
    }    

    public void Add(TEntity entity)
        => _entityDbSet.Add(entity);    

    public void SoftDelete(TEntity entity)
        => _entityDbSet.Remove(entity);    

    public void Update(TEntity entity)
        => _entityDbSet.Update(entity);

    public async Task<TEntity?> FirstOrDefaultAsync(CancellationToken ct, ISpecificaion<TEntity>? specificaion = null, bool TrackingEnabled = false)
    {
        if (specificaion is not null)
            return !TrackingEnabled ? await ApplySpecification(specificaion).AsNoTracking().FirstOrDefaultAsync(ct) : await ApplySpecification(specificaion).FirstOrDefaultAsync(ct);
        
        return !TrackingEnabled ? await _entityDbSet.AsNoTracking().FirstOrDefaultAsync(ct) : await _entityDbSet.FirstOrDefaultAsync(ct);
    }

    public async Task<bool> AnyAsync(CancellationToken ct, ISpecificaion<TEntity>? specificaion = null)
        => specificaion is not null ?
            await ApplySpecification(specificaion).AnyAsync(ct) :
            await _entityDbSet.AnyAsync(ct);

    public async Task<IEnumerable<TEntity>> ListAsync(ISpecificaion<TEntity> specificaion, CancellationToken ct, bool TrackingEnabled = false)
        => !TrackingEnabled ? await ApplySpecification(specificaion).AsNoTracking().ToListAsync(ct) : await ApplySpecification(specificaion).ToListAsync(ct);

    public async Task<int> SaveChangesAsync(CancellationToken ct)
        => await _gymDbContext.SaveChangesAsync(ct);

    private IQueryable<TEntity> ApplySpecification(ISpecificaion<TEntity> specificaion)
        => SpecificationEvaluator<TEntity>.GetQuery(_entityDbSet, specificaion);
}
