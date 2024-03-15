using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.Extensions.Configuration;
using Stripe;
using Product = Core.Entities.Product;

namespace Infrastructure.Services;

public class PaymentService : IPaymentService
{
    private readonly IBasketRepository _basketRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _config;

    public PaymentService(IBasketRepository basketRepository, IUnitOfWork unitOfWork, IConfiguration config)
    {
        _basketRepository = basketRepository;
        _unitOfWork = unitOfWork;
        _config = config;
    }


    public async Task<CustomerBasket> CreateOrUpdatePaymentIntent(string basketId)
    {
        StripeConfiguration.ApiKey = _config["StripeSettings:SecretKey"];

        CustomerBasket basket = await _basketRepository.GetBasketAsync(basketId);

        if (basket == null) return null;

        decimal shippingprice = 0m;

        if (basket.DeliveryMethodId.HasValue)
        {
            DeliveryMethods deliveryMethod = await _unitOfWork.Repository<DeliveryMethods>().GetByIdAsync((int)basket.DeliveryMethodId);
            shippingprice = deliveryMethod.Price;
        }

        foreach(BasketItem item in basket.Items)
        {
            Product productItem = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
            if (item.Price != productItem.Price)
            {
                item.Price = productItem.Price;
            }
        }

            PaymentIntentService service = new();

            PaymentIntent intent;

            if (string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                PaymentIntentCreateOptions options = new()
                {
                    Amount = (long) basket.Items.Sum(i => i.Quantity * (i.Price * 100)) + (long)shippingprice * 100,
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> {"card"} 

                };

                intent = await service.CreateAsync(options);
                basket.PaymentIntentId = intent.Id;
                basket.ClientSecret = intent.ClientSecret;

            }
            
            else
            {
                PaymentIntentUpdateOptions options = new()
                {
                    Amount = (long) basket.Items.Sum(i => i.Quantity * (i.Price * 100)) + (long)shippingprice * 100
                };

                await service.UpdateAsync(basket.PaymentIntentId, options);
            }

            await _basketRepository.UpdateBasketAsync(basket);

            return basket;
    }

    public async Task<Order> UpdateOrderPaymentFailed(string paymentIntentId)
    {
        OrderByPaymentIntentIdSpecification spec = new(paymentIntentId);

        Order order = await _unitOfWork.Repository<Order>().GetEntityWithSpec(spec);

        if (order == null) return null;

        order.Status = OrderStatus.PaymentFailed;

        await _unitOfWork.Complete();

        return order;
    }

    public async Task<Order> UpdateOrderPaymentSucceded(string paymentIntentId)
    {
        OrderByPaymentIntentIdSpecification spec = new(paymentIntentId);

        Order order = await _unitOfWork.Repository<Order>().GetEntityWithSpec(spec);

        if (order == null) return null;

        order.Status = OrderStatus.PaymentReceived;

        await _unitOfWork.Complete();


        return order;
    }
}
