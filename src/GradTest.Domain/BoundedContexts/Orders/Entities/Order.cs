namespace GradTest.Domain.BoundedContexts.Orders.Entities;

public class Order
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public List<OrderItem> Items { get; init; } = new();
}