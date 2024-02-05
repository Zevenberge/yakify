using System.ComponentModel.DataAnnotations;
using Yakify.Domain;

namespace Yakify.Api.Models;

public class OrderDto
{
    [Required(ErrorMessage = Errors.ORDER_MILK_CANNOT_BE_EMPTY)]
    [Min(0, ErrorMessage = Errors.ORDER_MILK_CANNOT_BE_NEGATIVE)]
    public double? Milk { get; set; }
    [Required(ErrorMessage = Errors.ORDER_SKINS_CANNOT_BE_EMPTY)]
    [Min(0, ErrorMessage = Errors.ORDER_SKINS_CANNOT_BE_NEGATIVE)]
    public int? Skins { get; set; }
}