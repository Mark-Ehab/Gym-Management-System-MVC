using GymManagementSystem.DataAccess.Models.BusinessModels;
using GymManagementSystem.DataAccess.Specifiction.Contract;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace GymManagementSystem.DataAccess.Specifiction.Classes;

public abstract class Specification<TEntity> : ISpecificaion<TEntity> where TEntity : BaseEntity, new()
{
    public Expression<Func<TEntity, bool>>? Criteria { get; }

    public List<Func<IQueryable<TEntity>, IQueryable<TEntity>>> IncludeLambdas { get; } = [];

    public List<OrderByInfo<TEntity>> OrderBys { get; } = [];

    public int Take { get; set; }

    public int Skip { get; set;}

    public bool IsPaginationEnabled { get; set;}
    protected Specification(Expression<Func<TEntity, bool>>? criteria = null)
    {
        Criteria = criteria;
    }

    protected void AddInclude(Func<IQueryable<TEntity>, IQueryable<TEntity>> includeLambda)
        => IncludeLambdas.Add(includeLambda);
    protected void AddOrderBy(OrderByInfo<TEntity> orderByData)
        => OrderBys.Add(orderByData);

}
