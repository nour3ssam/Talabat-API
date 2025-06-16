using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Interfaces;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreContext dbcontext;
        private Hashtable repos;
        public UnitOfWork(StoreContext _dbcontext)
        {
            repos = new Hashtable();
            dbcontext = _dbcontext;
        }
        public async Task<int> CompleteAsync()
        => await dbcontext.SaveChangesAsync();
        

        public async ValueTask DisposeAsync()
        => await dbcontext.DisposeAsync();
        

        public IGenericRepo<TEntity> Repo<TEntity>() where TEntity : BaseEntitiy
        {
            var type = typeof(TEntity).Name;
            if (!repos.ContainsKey(type))
            {
                var repo = new GenericRepo<TEntity>(dbcontext);
                repos.Add(type, repo);
            }
            // return (IGenericRepo <TEntity>) repos[type];
            return  repos[type] as IGenericRepo<TEntity>; // Casting


        }
    }
}
