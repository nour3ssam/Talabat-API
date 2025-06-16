using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Entities;
using Talabat.Core.Interfaces;
using TalabatAPI.DTOs;


namespace TalabatAPI.Controllers
{

    public class BasketController : APIBaseController
    {
        private readonly IBasketRepo basketRepo;
        private readonly IMapper mapper;

        public BasketController(IBasketRepo _basketRepo , IMapper _mapper)
        {
            basketRepo = _basketRepo;
            mapper = _mapper;
        }


        // Get or Create but item[ not container]
        [HttpGet]
        public async Task<ActionResult<CustomerBasket>> GetCustomerBasket(string id)
        {
            var Basket = await basketRepo.GetBasket(id);
            if (Basket is null) return new CustomerBasket(id);
            else return Ok(Basket);


        }


        // Update if id exists ,If not exists will Create New
        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasketDto Basket)
        {
            var MappedBasket = mapper.Map<CustomerBasketDto, CustomerBasket>(Basket);
            var CreateOrUpdate = await basketRepo.UpdateBasket(MappedBasket);
            return Ok(CreateOrUpdate);

        }


        // Delete
        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteBasket(string id)
        {
            return await basketRepo.DeleteBacket(id);   
        }

    }
}
