using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Core.Interfaces
{
    public interface IGenericRepo<T> where T : BaseEntitiy
    {
        #region Without Specification
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T> getByIdAsync(int id);
        #endregion


        Task AddAsync(T item);
        void UpdateAsync(T item);
        void DeleteAsync(T item);



        #region With Specification

        Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> Spec);
        Task<T> GetEntityWithSpecAsync(ISpecification<T> Spec);
        #endregion





    }
}

