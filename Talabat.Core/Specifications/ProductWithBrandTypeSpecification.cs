using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{
    public class ProductWithBrandTypeSpecification : BaseSpecifications<Product>
    {
        public ProductWithBrandTypeSpecification(ProductSpecParams Param) : 
            base(p=> 
            (string.IsNullOrEmpty(Param.Search) || p.Name == Param.Search)
            &&
             (!Param.TypeId.HasValue || p.ProductTypeId == Param.TypeId)
            &&  
            (!Param.BrandId.HasValue || p.ProductBrandId == Param.BrandId) 
            ) 
        {
            Includes.Add(p => p.ProductBrand);
            Includes.Add(P => P.ProductType);

            if (!string.IsNullOrEmpty(Param.Sort))
            {
                switch (Param.Sort)
                {
                    case "PriceAsc":
                        AddOrderBy(p => p.Price);
                        break;
                    case "PriceDesc":
                        AddOrderByDesc(p => p.Price);
                        break;
                    default:
                        AddOrderBy(p => p.Name);
                        break;
                }

            }

            ApplyPagination(Param.PageSize * (Param.PageIndex - 1), Param.PageSize);
            
        }
        public ProductWithBrandTypeSpecification(int id) : base(p=>p.Id==id)
        {
            Includes.Add(p => p.ProductBrand);
            Includes.Add(P => P.ProductType);

        }
    }
}
