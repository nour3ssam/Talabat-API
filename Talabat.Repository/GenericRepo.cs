using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Interfaces;
using Talabat.Core.Specifications;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public class GenericRepo<T> : IGenericRepo<T> where T : BaseEntitiy
    {
        private readonly StoreContext dbContext;

        public GenericRepo(StoreContext _dbcontext) {
             dbContext = _dbcontext;
        }

        #region Without Specification

        public async Task<T> getByIdAsync(int id)
         => await dbContext.Set<T>().Where(x => x.Id == id).FirstOrDefaultAsync();
        // => await dbContext.Set<T>().FindAsync(id);


        public async Task <IReadOnlyList<T>>GetAllAsync()
        {
            if(typeof(T)== typeof(Product))
            {
                return (IReadOnlyList<T>) await dbContext.Set<Product>().Include(p => p.ProductBrand).Include(p => p.ProductType).ToListAsync();
            }
            else
            {
                return await dbContext.Set<T>().ToListAsync();
            }
        }
        #endregion





        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> Spec) =>
            await SpecificationEvalutor<T>.GetQuery(dbContext.Set<T>(), Spec).ToListAsync();

        public async Task<T> GetEntityWithSpecAsync(ISpecification<T> Spec) =>
            await SpecificationEvalutor<T>.GetQuery(dbContext.Set<T>(), Spec).FirstOrDefaultAsync();




        public async Task AddAsync(T item)
       => await dbContext.Set<T>().AddAsync(item);
        
        public void UpdateAsync(T item)
              => dbContext.Set<T>().Update(item);

       public void DeleteAsync(T item)
             => dbContext.Set<T>().Remove(item);

    }
}
