using GymManagementSystem.DataAccess.Data.Contexts;
using GymManagementSystem.DataAccess.Models.BusinessModels;
using GymManagementSystem.DataAccess.Repositories.Contracts;
using GymManagementSystem.DataAccess.Specifiction.Classes;
using GymManagementSystem.DataAccess.Specifiction.Contract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace GymManagementSystem.DataAccess.Repositories.Classes;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity, new()
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
    public async Task<IReadOnlyList<TEntity>?> GetAllAsync(bool noTrackingEnabled = false, bool softDeletedItemsEnabled = false, CancellationToken ct = default)
    {
        var query = _entityDbSet.AsQueryable();

        if (noTrackingEnabled)
            query = query.AsNoTracking();

        if (softDeletedItemsEnabled)
            query = query.IgnoreQueryFilters();

        return await query.ToListAsync(ct);
    }
    
    public async Task<TEntity?> GetByIdAsync(Guid id, bool noTrackingEnabled = false, bool softDeletedItemsEnabled = false, CancellationToken ct = default)
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

    public async Task<TEntity?> FirstOrDefaultAsync(ISpecificaion<TEntity>? specificaion = null, bool TrackingEnabled = false, CancellationToken ct = default)
    {
        if (specificaion is not null)
            return !TrackingEnabled ? await ApplySpecification(specificaion).AsNoTracking().FirstOrDefaultAsync(ct) : await ApplySpecification(specificaion).FirstOrDefaultAsync(ct);
        
        return !TrackingEnabled ? await _entityDbSet.AsNoTracking().FirstOrDefaultAsync(ct) : await _entityDbSet.FirstOrDefaultAsync(ct);
    }

    public async Task<bool> AnyAsync(ISpecificaion<TEntity>? specificaion = null, CancellationToken ct = default)
        => specificaion is not null ?
            await ApplySpecification(specificaion).AnyAsync(ct) :
            await _entityDbSet.AnyAsync(ct);

    public async Task<IReadOnlyList<TEntity>> ListAsync(ISpecificaion<TEntity> specificaion, bool TrackingEnabled = false, CancellationToken ct = default)
        => !TrackingEnabled ? await ApplySpecification(specificaion).AsNoTracking().ToListAsync(ct) : await ApplySpecification(specificaion).ToListAsync(ct);

    public async Task<int> CountAsync(ISpecificaion<TEntity>? specificaion = null, CancellationToken ct = default)
        => specificaion is not null ? 
            await ApplySpecification(specificaion).CountAsync(ct) :
            await _entityDbSet.CountAsync();

    public async Task<int> SaveChangesAsync(CancellationToken ct)
        => await _gymDbContext.SaveChangesAsync(ct);

    private IQueryable<TEntity> ApplySpecification(ISpecificaion<TEntity> specificaion)
        => SpecificationEvaluator<TEntity>.GetQuery(_entityDbSet, specificaion);

}
