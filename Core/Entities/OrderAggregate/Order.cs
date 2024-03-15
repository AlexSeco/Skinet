using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Entities.OrderAggregate;

public class Order : BaseEntity
{
    public Order()
    {
    }

    public Order(IReadOnlyList<OrderItem> orderItems, string buyerEmail, Address shipToAdress, DeliveryMethods deliveryMethod, decimal subtotal, string paymentIntentId) 
    {
        BuyerEmail = buyerEmail;
        ShipToAdress = shipToAdress;
        DeliveryMethod = deliveryMethod;
        OrderItems = orderItems;
        Subtotal = subtotal;
        PaymentIntentId = paymentIntentId;
    }
    public string BuyerEmail { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public Address ShipToAdress { get; set; }
    public DeliveryMethods DeliveryMethod { get; set; }
    public IReadOnlyList<OrderItem> OrderItems { get; set; }
    public decimal Subtotal { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public string? PaymentIntentId { get; set; } 

    public decimal GetTotal() //Automapper knows when a function has the name with Get at the beginning so it maps this to a property called total
    {
        return Subtotal + DeliveryMethod.Price;
    }

}