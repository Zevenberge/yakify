using System.ComponentModel.DataAnnotations;
using Yakify.Domain;

namespace Yakify.Api.Models;

public class PlaceOrderDto
{
    [Required(ErrorMessage = Errors.ORDER_CUSTOMER_CANNOT_BE_EMPTY)]
    public string? Customer { get; set; }
    [Required(ErrorMessage = Errors.ORDER_PRODUCTS_CANNOT_BE_EMPTY)]
    public OrderDto? Order { get; set; }
}
