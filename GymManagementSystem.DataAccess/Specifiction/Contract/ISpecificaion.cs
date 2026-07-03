using GymManagementSystem.DataAccess.Models.BusinessModels;
using GymManagementSystem.DataAccess.Specifiction.Classes;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace GymManagementSystem.DataAccess.Specifiction.Contract;

public interface ISpecificaion<TEntity> where TEntity : BaseEntity , new()
{
    Expression<Func<TEntity,bool>>? Criteria { get; }
    List<Func<IQueryable<TEntity>, IQueryable<TEntity>>> IncludeLambdas { get; }
    List<OrderByInfo<TEntity>> OrderBys { get; }
    int Take { get; }
    int Skip { get; }
    bool IsPaginationEnabled { get; }
}
