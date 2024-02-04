namespace Yakify.Domain.Tests;

public class OrderTests
{
    [Fact]
    public void Order_is_initialized_correctly()
    {
        var order = new Order("Garry", 1.4, 2);
        order.Customer.Should().Be("Garry");
        order.Milk.Should().Be(1.4);
        order.Skins.Should().Be(2);
    }

    [Fact]
    public void Order_without_milk_is_valid()
    {
        var order = new Order("Garry", null, 2);
        order.Milk.Should().BeNull();
    }

    [Fact]
    public void Order_without_skins_is_valid()
    {
        var order = new Order("Garry", 1.4, null);
        order.Skins.Should().BeNull();
    }

    [Fact]
    public void Order_with_zero_ordered_is_valid()
    {
        var order = new Order("Garry", 0, 0);
        order.Milk.Should().NotBeNull();
        order.Milk.Should().Be(0);
        order.Skins.Should().NotBeNull();
        order.Skins.Should().Be(0);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData(" ")]
    public void Order_cannot_have_no_customer(string? customer)
    {
        FluentActions.Invoking(() => new Order(customer!, 2, 4))
            .Should().Throw<YakException>().WithMessage(Errors.ORDER_CUSTOMER_CANNOT_BE_EMPTY);
    }
}