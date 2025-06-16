using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.Core;
using Talabat.Core.Entities.Order;
using Talabat.Core.ServiceInterfaces;
using TalabatAPI.DTOs;

namespace TalabatAPI.Controllers
{
    public class OrderController : APIBaseController
    {
        private readonly IOrderService orderService;
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;

        public OrderController(IOrderService _OrderService , IMapper _mapper, IUnitOfWork _unitOfWork)
        {
            orderService = _OrderService;
            mapper = _mapper;
            unitOfWork = _unitOfWork;
        }

        //Create Order
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(OrderDto order)
        {
            var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var MappedAddress = mapper.Map<AddressDto, OrderAddress>(order.ShippingAddress);
          var Order = await orderService.CreateOrderAsync(BuyerEmail, order.CustomerBasketId, order.DeliveryMethodId, MappedAddress);
            if (Order == null) { return BadRequest(); }
            return Ok(Order);       
        }



        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrdersForUser()
        {
            var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var Orders = await orderService.GetAllOrdersOfUserAsync(BuyerEmail);
            if(Orders == null) { return NotFound(); }
            var mappedOrders = mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderToReturnDto>>(Orders);
            return Ok(mappedOrders);  
        }



        [Authorize]
        [HttpGet("{Id}")]
        public async Task<ActionResult<OrderToReturnDto>> GetOrderByIdForUser(int id)
        {
            var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var Order = await orderService.GetOrderByIdOfUserAsync(BuyerEmail,id);
            if (Order == null) { return NotFound(); }
            var MappedOrder = mapper.Map<Order, OrderToReturnDto>(Order);
            return Ok(MappedOrder);
        }



        [HttpGet("DeliveryMethods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
        {
            var DeliveryMethods =await unitOfWork.Repo<DeliveryMethod>().GetAllAsync();
            return Ok(DeliveryMethods);

        }



    }
}
