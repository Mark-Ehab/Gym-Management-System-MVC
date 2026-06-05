using GymManagementSystem.DataAccess.Models;
using GymManagementSystem.DataAccess.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.DataAccess.UoW.Contract;

public interface IUnitOfWork
{
    IGenericRepository<TEntity> GetGenericRepository<TEntity>() where TEntity : BaseEntity, new();
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
