using GradTest.Domain.BoundedContexts.Products.Entities;

namespace GradTest.Domain.BoundedContexts.Orders.Entities;

public class OrderItem
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid OrderId { get; init; }
    public Guid ProductId { get; init; }
    public int Quantity { get; init; }
    
    public Order Order { get; init; } = null!;
    public Product Product { get; init; } = null!;
    
    public OrderItem() {}

    public OrderItem(Guid orderId, Guid productId, int quantity)
    {
        OrderId = orderId;
        ProductId = productId;
        Quantity = quantity;
    }
}