using Yakify.Api.CodeAnalysis;
using Yakify.Api.Models;
using Yakify.Domain;
using Yakify.Repository;

namespace Yakify.Api.Services;

public class OrderService(IOrderRepository orderRepository, StockService stockService)
{
    public async Task<OrderDto?> PlaceOrder(int day, PlaceOrderDto orderDto, CancellationToken cancellationToken)
    {
        Assume.NotNull(orderDto.Customer);
        Assume.NotNull(orderDto.Order);
        var stock = await stockService.GetStock(day, cancellationToken);
        var normalizedOrder = LimitByStock(orderDto.Order, stock);
        if(normalizedOrder.Milk == null && normalizedOrder.Skins == null)
        {
            return null;
        }
        var order = new Order(orderDto.Customer, 
            normalizedOrder.Milk, 
            normalizedOrder.Skins);
        await orderRepository.Add(order, cancellationToken);
        return normalizedOrder;
    }

    private static OrderDto LimitByStock(OrderDto orderDto, StockDto stockDto)
    {
        return new OrderDto
        {
            Milk = Limit(orderDto.Milk, stockDto.Milk),
            Skins = Limit(orderDto.Skins, stockDto.Skins),
        };
    }

    private static T? Limit<T>(T? order, T stock) where T: struct, IComparable<T>
    {
        if(order == null) return null;
        if(order.Value.CompareTo(stock) > 0) return null;
        return order;
    }
}