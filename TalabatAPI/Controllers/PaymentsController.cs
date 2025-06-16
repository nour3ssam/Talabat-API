using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.ServiceInterfaces;
using TalabatAPI.DTOs;

namespace TalabatAPI.Controllers
{
  
    public class PaymentsController : APIBaseController
    {
        private readonly IPaymentService paymentService;

        public PaymentsController(IPaymentService _paymentService)
        {
            paymentService = _paymentService;
        }
        [HttpPost]
        public async Task<ActionResult<CustomerBasketDto>> CreateOrUpdatePaymentIntent(string BasketId)
        {
            var CustomerBasket =await paymentService.CreateOrUpdatePaymentIntent(BasketId);
            if (CustomerBasket == null) { return BadRequest(); }
            return Ok(CustomerBasket);
            





        }
    }
}
