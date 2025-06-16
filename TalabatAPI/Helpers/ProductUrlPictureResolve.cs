using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using Talabat.Core.Entities;
using TalabatAPI.DTOs;

namespace TalabatAPI.Helpers
{
    public class ProductUrlPictureResolve : IValueResolver<Product, ProductToReturnDTO, string>
    {

        private readonly IConfiguration Configuration;
        public ProductUrlPictureResolve(IConfiguration _Configuration)
        {
            Configuration = _Configuration;
        }

        public string Resolve(Product source, ProductToReturnDTO destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.PictureURL)){
                return $"{Configuration["BaseUrl"]}{source.PictureURL}";
            }
            else
            {
                return string.Empty ;
            }
        }
    }
}
