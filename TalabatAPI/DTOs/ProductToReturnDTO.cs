using Talabat.Core.Entities;

namespace TalabatAPI.DTOs
{
    public class ProductToReturnDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PictureURL { get; set; }
        public decimal Price { get; set; }
        public int ProductTypeId { get; set; }// Forign Key
        public string ProductType { get; set; } // Navigational Property

        public int ProductBrandId { get; set; }// Forign Key
        public string ProductBrand { get; set; } // Navigational Property
    }
}
