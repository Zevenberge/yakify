using Yakify.Api.Models;
using Yakify.Domain;

namespace Yakify.Api.Tests.Models;

public class PlaceOrderDtoTests
{
    [Fact]
    public void Place_order_is_valid()
    {
        var dto = new PlaceOrderDto
        {
            Customer = "Garry",
            Order = new OrderDto
            {
                Milk = 2000,
                Skins = 1
            }
        };
        dto.ShouldBeValid();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Place_order_without_customer_is_not_valid(string? customer)
    {
        var dto = new PlaceOrderDto
        {
            Customer = customer,
            Order = new OrderDto
            {
                Milk = 2000,
                Skins = 1
            }
        };
        dto.ShouldHaveError(nameof(PlaceOrderDto.Customer), Errors.ORDER_CUSTOMER_CANNOT_BE_EMPTY);
    }

    [Fact]
    public void Place_order_without_products_is_not_valid()
    {
        var dto = new PlaceOrderDto
        {
            Customer = "Garry",
            Order = null
        };
        dto.ShouldHaveError(nameof(PlaceOrderDto.Order), Errors.ORDER_PRODUCTS_CANNOT_BE_EMPTY);
    }
}
