namespace Yakify.Domain;

public class Order
{
    private Order() {}

    public Order(string customer, double? milk, int? skins, int day)
    {
        ThrowOnInvalidInput(customer, day);
        Customer = customer;
        Milk = milk;
        Skins = skins;
        Day = day;
    }

    private static void ThrowOnInvalidInput(string customer, int day)
    {
        if(string.IsNullOrWhiteSpace(customer)) throw new YakException(Errors.ORDER_CUSTOMER_CANNOT_BE_EMPTY);
        if(day < 0) throw new YakException(Errors.ORDER_DAY_CANNOT_BE_NEGATIVE);
    }

    public int Id { get; private set; }
    public string Customer {get; private set; } = "";
    public double? Milk { get; private set; }
    public int? Skins { get; private set; }
    public int Day { get; private set; }
}