using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Security.Claims;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Services;
using TalabatAPIProject.Dtos;
using TalabatAPIProject.Error;
using Order = Talabat.Core.Entities.Order_Aggregate.Order;

namespace TalabatAPIProject.Controllers
{
    [Authorize]
    public class OrdersController : BaseApiController
    {
        private readonly IOrderServices _orderService;
        private readonly IMapper _mapper;

        public OrdersController(IOrderServices orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        //[ProducesResponseType(typeof(Order),statusCode:200)]
        //[ProducesResponseType(typeof(ApiResponse),StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<ActionResult<OrderToReturnDto>> CreateOrder(OrderDto orderDto)
        {

            string buyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var orderAddress = _mapper.Map<AddressDto, Address>(orderDto.ShippingAddress);

            var order = await _orderService.CreateOrderAsync(buyerEmail, orderDto.BasketId, orderDto.DeliveryMethodId, orderAddress);

            if (order == null) return BadRequest(new ApiResponse(400, "An error occured during the creation of the order"));

            return Ok(_mapper.Map<Order,OrderToReturnDto>(order));
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrderForUserAsync()
        {
            string buyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var orders = await _orderService.GetOrdersForUser(buyerEmail);

            return Ok(_mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderToReturnDto>>(orders));

        }
        [ProducesResponseType(typeof(OrderToReturnDto),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [HttpGet("{id}")] // Get: /api/orders/1?email=ahmed.nasr@linkdiv.com
        public async Task<ActionResult<OrderToReturnDto>> GetOrderForUserAsync(int id)
        {
            string buyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var orders = await _orderService.GetOrderByIdForUser(id,buyerEmail);
            if (orders is null)
                return NotFound(new ApiResponse(404));
            
            return Ok(_mapper.Map<OrderToReturnDto>(orders));
        }
        [HttpGet("deliveryMethods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
        {
            var deliveryMethods = await _orderService.GetDeliveryMethodAsync();
            return Ok(deliveryMethods);
        }
        
            //[HttpGet]
        //public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrdersForUser()
        //{
        //    string buyerEmail = User.FindFirstValue(ClaimTypes.Email);
        //    var orders = await _orderService.GetOrdersForUser(buyerEmail);
        //    return Ok(_mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderToReturnDto>>(orders));
        //}
        //[HttpGet("{id}")]
        //public async Task<ActionResult<OrderToReturnDto>> GetOrderForUser(int id)
        //{
        //    string buyerEmail = User.FindFirstValue(ClaimTypes.Email);
        //    var order = await _orderService.GetOrderById(id, buyerEmail);
        //    return Ok(_mapper.Map<Order, OrderToReturnDto>(order));
        //}

        //[HttpGet("deliveryMethods")]
        //public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
        //{
        //    var deliveryMethods = await _orderService.GetDeliveryMethodAsync();
        //    return Ok(deliveryMethods);
        //}
    }
}