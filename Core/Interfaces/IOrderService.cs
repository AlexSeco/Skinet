using Core.Entities.OrderAggregate;

namespace Core.Interfaces;

public interface IOrderService
{
    Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethod, string basketId, Address shippingAdress);

    Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail);

    Task<Order> GetOrderByIdAsync(int id, string buyerEmail);

    Task<IReadOnlyList<DeliveryMethods>> GetDeliveryMethodsAsync();
}