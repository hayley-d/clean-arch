using GradTest.Domain.Common.Entities;
using GradTest.Shared.Monads;

namespace GradTest.Domain.BoundedContexts.Orders.Entities;

public class Order : EntityBase
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public List<OrderItem> Items { get; init; } = new();
    
    public DateTime Created { get; init; }
    
    private Order() {}

    private Order(Guid userId, List<OrderItem> items)
    {
        Id = new Guid();
        Created = DateTime.UtcNow;
        UserId = userId;
        Items = items;
    }

    public static Order Create( Guid userId, List<OrderItem> items)
    {
        var order = new Order(userId, items);
        
        return Result<Order>.Success(order);
    }
    
}