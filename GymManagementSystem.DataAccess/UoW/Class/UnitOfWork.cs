using GymManagementSystem.DataAccess.Data.Contexts;
using GymManagementSystem.DataAccess.Models.BusinessModels;
using GymManagementSystem.DataAccess.Repositories.Classes;
using GymManagementSystem.DataAccess.Repositories.Contracts;
using GymManagementSystem.DataAccess.UoW.Contract;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.DataAccess.UoW.Class;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly GymDbContext _gymDbContext;
    private readonly Dictionary<string, object> _repositories = []; 
    public ISessionRepository SessionRepository { get; }

    public UnitOfWork(GymDbContext gymDbContext, ISessionRepository sessionRepository)
    {
        _gymDbContext = gymDbContext;
        SessionRepository = sessionRepository;
    }

    public IGenericRepository<TEntity> GetGenericRepository<TEntity>() where TEntity : BaseEntity, new()
    {
        var repositoryKey = typeof(TEntity).Name;

        /* Check if requested generic repository already exists */
        if (_repositories.TryGetValue(repositoryKey, out object? repository))
            return (IGenericRepository<TEntity>) repository!;

        /* Create a new generic repository then add it to _repositories dictionary */
        var newGenericRepository = new GenericRepository<TEntity>(_gymDbContext);
        _repositories[repositoryKey] = newGenericRepository;
        return newGenericRepository;
    }

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        => await _gymDbContext.SaveChangesAsync();
}
