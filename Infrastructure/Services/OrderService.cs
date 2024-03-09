using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;

namespace Infrastructure.Services;

public class OrderService : IOrderService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBasketRepository _basketRepo;

    public OrderService(IUnitOfWork unitOfWork, IBasketRepository basketRepo)
    {
        _unitOfWork = unitOfWork;
        _basketRepo = basketRepo;
    }

    public async Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string basketId, Address shippingAdress)
    {
        // get basket from the basket repo
        CustomerBasket basket = await _basketRepo.GetBasketAsync(basketId);

        // get items from the product repo
        List<OrderItem> items = new();

        foreach (BasketItem item in basket.Items)
        {
            Product productItem = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);

            ProductItemOrdered itemOrdered = new(productItem.Id, productItem.Name, productItem.PictureUrl);

            OrderItem orderItem = new(itemOrdered, productItem.Price, item.Quantity);

            items.Add(orderItem);
        }

        // get delivery method from repo

        DeliveryMethods deliveryMethod = await _unitOfWork.Repository<DeliveryMethods>().GetByIdAsync(deliveryMethodId);

        // calculate subtotal

        decimal subtotal = items.Sum(item => item.Price * item.Quantity);
        // create order

        Order order = new(items, buyerEmail, shippingAdress, deliveryMethod, subtotal);

        // TO DO: save to db

        _unitOfWork.Repository<Order>().Add(order);
        var result = await _unitOfWork.Complete();

        if (result <= 0) return null;

        //delete basket
        await _basketRepo.DeleteBasketAsync(basketId);

        return order;
    }

    public async Task<IReadOnlyList<DeliveryMethods>> GetDeliveryMethodsAsync()
    {
        return await _unitOfWork.Repository<DeliveryMethods>().ListAllAsync(); 
    }

    public async Task<Order> GetOrderByIdAsync(int id, string buyerEmail)
    {
        OrderWithDeliveryMethodAndOrderItems spec = new(id, buyerEmail);

        return await _unitOfWork.Repository<Order>().GetEntityWithSpec(spec);

    }

    public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
    {
        OrderWithDeliveryMethodAndOrderItems spec = new(buyerEmail);

        return await _unitOfWork.Repository<Order>().ListAsync(spec);
    }
}
