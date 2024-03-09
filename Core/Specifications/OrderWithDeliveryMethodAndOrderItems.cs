using Core.Entities.OrderAggregate;

namespace Core.Specifications;

public class OrderWithDeliveryMethodAndOrderItems : BaseSpecification<Order>
{
    public OrderWithDeliveryMethodAndOrderItems(string email) : base(o => o.BuyerEmail == email)
    {
        AddInclude(x => x.OrderItems);
        AddInclude(x => x.DeliveryMethod);

        AddOrderbyDescending(x => x.OrderDate);

    }

    public OrderWithDeliveryMethodAndOrderItems(int id, string email) : base(o => o.Id == id && o.BuyerEmail == email)
    {
        AddInclude(x => x.OrderItems);
        AddInclude(x => x.DeliveryMethod);
    }
}