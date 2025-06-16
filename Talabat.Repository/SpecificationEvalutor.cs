using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{
    public static class SpecificationEvalutor<T> where T : BaseEntitiy
    {

        // dbContext.Set<Product>().Include(p => p.ProductBrand).Include(p => p.ProductType).ToListAsync();
        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery , ISpecification<T> Spec)
        {
            var Query = inputQuery; //dbContext.Set<T>()
            if(Spec.Criteria is not null)
            {
                Query = Query.Where(Spec.Criteria); //dbContext.Set<T>().Where(Criteria)
            }
            if(Spec.OrderBy is not null)
            {
                Query = Query.OrderBy(Spec.OrderBy);
            }
            if(Spec.OrderBydesc is not null)
            {
                Query= Query.OrderByDescending(Spec.OrderBydesc);
            }
            if (Spec.IsPaginationEnabled)
            {
                Query = Query.Skip(Spec.Skip).Take(Spec.Take); 
            }


            // .Include(p => p.ProductBrand).Include(p => p.ProductType)

            Query = Spec.Includes.Aggregate(Query, (CurrentQuery, IncludeExpression) => CurrentQuery.Include(IncludeExpression));

            return Query;



        }
    }
}
