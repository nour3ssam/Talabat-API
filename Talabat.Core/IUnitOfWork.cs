using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Interfaces;

namespace Talabat.Core
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IGenericRepo<TEntity> Repo<TEntity>() where TEntity : BaseEntitiy; 

        Task<int> CompleteAsync();
    }
}
