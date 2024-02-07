using Microsoft.AspNetCore.Mvc;
using Yakify.Api.Models;
using Yakify.Api.Services;

namespace Yakify.Api.Controllers;

[ApiController]
public class CustomerController: ControllerBase
{
    /// <summary>
    /// Places an order at the yak produce shop for the given day.
    /// </summary>
    /// <param name="day">The day at which to place the order</param>
    /// <param name="dto">The order to place</param>
    /// <param name="orderService"></param>
    /// <param name="cancellationToken"></param>
    /// <response code="201">Order was succesfully placed</response>
    /// <response code="206">Order was partially placed</response>
    /// <response code="404">Order was not placed due to insufficient stock</response>
    [HttpPost]
    [Route("/yak-shop/order/{day}")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(OrderDto))]
    [ProducesResponseType(StatusCodes.Status206PartialContent, Type = typeof(OrderDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Notification(typeof(StockHub))]
    public async Task<IActionResult> Order(
        [FromRoute] ushort day,
        [FromBody] PlaceOrderDto dto,
        [FromServices] OrderService orderService,
        CancellationToken cancellationToken)
    {
        var order = await orderService.PlaceOrder(day, dto, cancellationToken);
        if(order == null) return NotFound();
        if(order.Milk == null || order.Skins == null)
        {
            return StatusCode(StatusCodes.Status206PartialContent, order);
        }
        return StatusCode(StatusCodes.Status201Created, order);
    }
}