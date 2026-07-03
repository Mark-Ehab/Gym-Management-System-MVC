using GymManagementSystem.DataAccess.Models.BusinessModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace GymManagementSystem.DataAccess.Specifiction.Classes;

public class OrderByInfo<TEntity> where TEntity : BaseEntity, new()
{
    public Expression<Func<TEntity, object>> OrderByExpression { get; } 
    public bool IsDescending { get; } = false;
    public OrderByInfo(Expression<Func<TEntity, object>> orderByExpression, bool isDescending)
    {
        OrderByExpression = orderByExpression;
        IsDescending = isDescending;
    }
}
