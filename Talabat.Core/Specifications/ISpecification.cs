using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{
    public interface ISpecification<T> where T : BaseEntitiy
    {
      public Expression<Func<T,bool>> Criteria { set ; get; }
      public Expression<Func<T, object>> OrderBy { set; get; }
      public Expression<Func<T, object>> OrderBydesc { set; get; }

      public List<Expression<Func<T,object>>> Includes { get; set; }
        public int  Skip { get; set; }
        public int Take { get; set; }
        public bool IsPaginationEnabled { get; set; }



    }
}
