using GradTest.Shared.Monads;

namespace GradTest.Domain.BoundedContexts.Orders.Entities;

public class Order 
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

    public static Order Create( Guid userId, Dictionary<Guid,int> items)
    {
        var orderId = Guid.NewGuid();
        var convertedItems = new List<OrderItem>();
        foreach(var pair in items)
        {
            convertedItems.Add(new OrderItem(orderId, pair.Key, pair.Value));
        }
        var order = new Order(userId, convertedItems);
        
        return Result<Order>.Success(order);
    }
    
}