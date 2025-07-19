namespace GradTest.Contracts.Orders.Responses;

public class OrderResponse
{
     public Guid OrderId { get; init; }
     public Guid CustomerId { get; init; }
     public Dictionary<Guid, int> Items { get; init; }
}