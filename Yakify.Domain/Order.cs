namespace Yakify.Domain;

public class Order
{
    private Order() {}

    public Order(string customer, double? milk, int? skins)
    {
        ThrowOnInvalidInput(customer);
        Customer = customer;
        Milk = milk;
        Skins = skins;
    }

    private static void ThrowOnInvalidInput(string customer)
    {
        if(string.IsNullOrWhiteSpace(customer)) throw new YakException(Errors.ORDER_CUSTOMER_CANNOT_BE_EMPTY);
    }

    public int Id { get; private set; }
    public string Customer {get; private set; } = "";
    public double? Milk { get; private set; }
    public int? Skins { get; private set; }
}