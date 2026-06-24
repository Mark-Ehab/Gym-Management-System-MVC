using GymManagementSystem.DataAccess.Models.BusinessModels;
using GymManagementSystem.DataAccess.Specifiction.Contract;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.DataAccess.Specifiction.Classes;

public static class SpecificationEvaluator<TEntity> where TEntity : BaseEntity, new()
{
    public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> baseQuery, ISpecificaion<TEntity> specification)
    {
        IQueryable<TEntity> query = baseQuery;

        if(specification.Criteria is not null)
            query = query.Where(specification.Criteria);

        if (specification.IncludeLambdas.Count > 0)
            query = specification.IncludeLambdas.Aggregate(query,(current, Include)
                    => Include(current));

        if(specification.OrderBys.Count > 0)
        {
            var firstOrderBy = specification.OrderBys.FirstOrDefault();
            var orderedQuery = firstOrderBy!.IsDescending ? 
                query.OrderByDescending(firstOrderBy.OrderByExpression) :
                query.OrderBy(firstOrderBy.OrderByExpression);

            for (int orderExpressionCounter = 1; orderExpressionCounter < specification.OrderBys.Count; orderExpressionCounter++)
                orderedQuery = specification.OrderBys[orderExpressionCounter].IsDescending ?
                   orderedQuery.ThenByDescending(specification.OrderBys[orderExpressionCounter].OrderByExpression) :
                   orderedQuery.ThenBy(specification.OrderBys[orderExpressionCounter].OrderByExpression);
           
            query = orderedQuery;
        }

        if(specification.IsPaginationEnabled)
            query = query.Skip(specification.Skip).Take(specification.Take);

        return query; 
    }
}
