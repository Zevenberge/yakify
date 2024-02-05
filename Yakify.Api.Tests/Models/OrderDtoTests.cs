using Yakify.Api.Models;
using Yakify.Domain;

namespace Yakify.Api.Tests.Models;

public class OrderDtoTests
{
    [Fact]
    public void Order_is_valid()
    {
        var dto = new OrderDto
        {
            Milk = 20,
            Skins = 1
        };
        dto.ShouldBeValid();
    }

    [Fact]
    public void Order_without_milk_is_not_valid()
    {
        var dto = new OrderDto
        {
            Milk = null,
            Skins = 1
        };
        dto.ShouldHaveError(nameof(OrderDto.Milk), Errors.ORDER_MILK_CANNOT_BE_EMPTY);
    }

    [Fact]
    public void Order_with_negative_milk_is_not_valid()
    {
        var dto = new OrderDto
        {
            Milk = -0.01,
            Skins = 1
        };
        dto.ShouldHaveError(nameof(OrderDto.Milk), Errors.ORDER_MILK_CANNOT_BE_NEGATIVE);
    }

    [Fact]
    public void Order_without_skins_is_not_valid()
    {
        var dto = new OrderDto
        {
            Milk = 20,
            Skins = null
        };
        dto.ShouldHaveError(nameof(OrderDto.Skins), Errors.ORDER_SKINS_CANNOT_BE_EMPTY);
    }

    [Fact]
    public void Order_with_negative_skins_is_not_valid()
    {
        var dto = new OrderDto
        {
            Milk = 20,
            Skins = -1
        };
        dto.ShouldHaveError(nameof(OrderDto.Skins), Errors.ORDER_SKINS_CANNOT_BE_NEGATIVE);
    }

    [Fact]
    public void Order_with_0_products_is_valid()
    {
        var dto = new OrderDto
        {
            Milk = 0,
            Skins = 0
        };
        dto.ShouldBeValid();
    }
}