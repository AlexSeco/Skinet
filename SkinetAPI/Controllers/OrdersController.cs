using System.Security.Claims;
using AutoMapper;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkinetAPI.DTOs;
using SkinetAPI.Errors;
using SkinetAPI.Extensions;

namespace SkinetAPI.Controllers;

[Authorize]
public class OrdersController : BaseController
{
    private readonly IOrderService _orderService;
    private readonly IMapper _mapper;

    public OrdersController(IOrderService orderService, IMapper mapper)
    {
        _orderService = orderService;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<ActionResult<Order>> CreateOrder(OrderDTO orderDTO)
    {
        string email = HttpContext.User?.RetrieveEmail();

        Address address = _mapper.Map<AddressDTO, Address>(orderDTO.ShipToAdress);

        Order order = await _orderService.CreateOrderAsync(email, orderDTO.DeliveryMethodId, orderDTO.BasketId, address);

        if (order==null) return BadRequest(new APIResponse(400, "Problem creating order"));
        
        return Ok(order);
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<OrderToReturnDTO>>> GetOrdersForUser()
    {
        string email = HttpContext.User.RetrieveEmail();

        var orders = await _orderService.GetOrdersForUserAsync(email);

        return Ok(_mapper.Map<IReadOnlyList<OrderToReturnDTO>>(orders));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OrderToReturnDTO>> GetOrderByIdForUser(int id)
    {
        string email = HttpContext.User.RetrieveEmail();

        Order order = await _orderService.GetOrderByIdAsync(id, email);

        if (order == null) return NotFound(new APIResponse(404));

        return Ok(_mapper.Map<IReadOnlyList<OrderToReturnDTO>>(order));
    }

    [HttpGet("deliveryMethods")]
    public async Task<ActionResult<IReadOnlyList<DeliveryMethods>>> GetDeliveryMethods()
    {
        return Ok(await _orderService.GetDeliveryMethodsAsync());
    }
}