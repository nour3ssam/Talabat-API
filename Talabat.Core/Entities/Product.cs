using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities
{
    public class Product : BaseEntitiy
    {
        public string Name { get; set; }
        public string Description { get; set; }
       
        public string PictureURL { get; set; }
        public decimal Price { get; set; }


        public int ProductTypeId { get; set; }// Foreign Key
        public ProductType ProductType { get; set; } // Navigational Property

        public int ProductBrandId { get; set; }// Foreign Key
        public ProductBrand ProductBrand { get; set; } // Navigational Property

    }


}
