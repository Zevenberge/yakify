namespace Yakify.Domain.Tests;

public class OrderTests
{
    [Fact]
    public void Order_is_initialized_correctly()
    {
        var order = new Order("Garry", 1.4, 2, 15);
        order.Customer.Should().Be("Garry");
        order.Milk.Should().Be(1.4);
        order.Skins.Should().Be(2);
        order.Day.Should().Be(15);
    }

    [Fact]
    public void Order_without_milk_is_valid()
    {
        var order = new Order("Garry", null, 2, 12);
        order.Milk.Should().BeNull();
    }

    [Fact]
    public void Order_without_skins_is_valid()
    {
        var order = new Order("Garry", 1.4, null, 12);
        order.Skins.Should().BeNull();
    }

    [Fact]
    public void Order_with_zero_ordered_is_valid()
    {
        var order = new Order("Garry", 0, 0, 12);
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
        FluentActions.Invoking(() => new Order(customer!, 2, 4, 12))
            .Should().Throw<YakException>().WithMessage(Errors.ORDER_CUSTOMER_CANNOT_BE_EMPTY);
    }

    [Fact]
    public void Order_day_cannot_be_negative()
    {
        FluentActions.Invoking(() => new Order("Gary", 2, 4, -1))
            .Should().Throw<YakException>().WithMessage(Errors.ORDER_DAY_CANNOT_BE_NEGATIVE);
    }

    [Fact]
    public void Order_day_0_is_a_valid_day()
    {
        FluentActions.Invoking(() => new Order("Gary", 2, 4, 0))
            .Should().NotThrow();
    }
}