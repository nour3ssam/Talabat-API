using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Talabat.Core.Entities;
using Talabat.Repository.Data;
using TalabatAPI.DTOs;
using Talabat.Core.Specifications;
using Talabat.Core.Interfaces;
using TalabatAPI.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace TalabatAPI.Controllers
{

    public class ProductController : APIBaseController
    {

        private readonly IGenericRepo<Product> ProductRepo;
        private readonly IGenericRepo<ProductType> productTypeRepo;
        private readonly IGenericRepo<ProductBrand> productBrandRepo;
        private readonly IMapper mapper;

        public ProductController(
            IGenericRepo<Product> _ProductRepo , 
            IGenericRepo<ProductType> _ProductTypeRepo,
            IGenericRepo<ProductBrand> _ProductBrandRepo,
            IMapper _mapper
            ) {
            ProductRepo = _ProductRepo;
            productTypeRepo = _ProductTypeRepo;
            productBrandRepo = _ProductBrandRepo;
            mapper = _mapper;
        }



        // BaseURl > api/Product -> GET
        [Cached(300)]
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Pagination<ProductToReturnDTO>>>> Getproducts([FromQuery]ProductSpecParams Param)
        {
            var Spec = new ProductWithBrandTypeSpecification(Param);
            var products = await ProductRepo.GetAllWithSpecAsync(Spec);
            
             var ProductsMapper = mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDTO>>(products);
            var ReturnObject = new Pagination<ProductToReturnDTO>()
            {
                PageSize = Param.PageSize,
                PageIndex = Param.PageIndex,
                Count = 0,
                Data = ProductsMapper,
            };
             return Ok(ReturnObject);
        }




        // BaseURl > api/Product/id -> GET
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDTO>> Getproduct(int id)
        {
            var Spec = new ProductWithBrandTypeSpecification(id);
            var product = await ProductRepo.GetEntityWithSpecAsync(Spec);
            

            var ProductMapper = mapper.Map<Product, ProductToReturnDTO>(product);
            return Ok(ProductMapper);
        }





        [HttpGet("Types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
            var ProductTypes = await productTypeRepo.GetAllAsync();
            return Ok(ProductTypes);
        }

        [HttpGet("Brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
            var ProductBrandes = await productBrandRepo.GetAllAsync();
            return Ok(ProductBrandes);
        }






    }
}
