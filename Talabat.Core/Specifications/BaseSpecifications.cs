﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{
    public class BaseSpecifications<T> : ISpecification<T> where T : BaseEntitiy
    {
        public Expression<Func<T, bool>> Criteria { set; get; }
        public Expression<Func<T, object>> OrderBy { get; set ; }
        public Expression<Func<T, object>> OrderBydesc { get; set; }
        public List<Expression<Func<T, object>>> Includes { set; get; } = new List<Expression<Func<T, object>>>();
        public int Skip { get; set; }
        public int Take { get; set; }
        public bool IsPaginationEnabled { get; set; }

        public BaseSpecifications()
        {

        }
        public BaseSpecifications(Expression<Func<T, bool>> criteriaExpression)
        {
            Criteria = criteriaExpression;
        }

        public void AddOrderBy(Expression<Func<T, object>> OrderByExpression)
        {
            OrderBy = OrderByExpression;
        }
        public void AddOrderByDesc(Expression<Func<T, object>> OrderByDescExpression)
        {
            OrderBydesc = OrderByDescExpression;
        }
        public void ApplyPagination(int _skip, int _take)
        {
            IsPaginationEnabled = true;
            Skip=_skip;
            Take=_take; 
        }


    }
}
