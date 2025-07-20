using GradTest.Domain.BoundedContexts.Products.Entities;

namespace GradTest.Domain.BoundedContexts.Orders.Entities;

public class OrderItem
{
    public Guid Id { get; init; }
    public Guid OrderId { get; init; }
    public Guid ProductId { get; init; }
    public int Quantity { get; init; }
    
    public Order Order { get; init; } = null!;
    public Product Product { get; init; } = null!;
}