using GradTest.Domain.BoundedContexts.Orders.Entities;

namespace GradTest.Contracts.Orders.Responses;

public class OrderResponse
{
     public Guid OrderId { get; init; }
     public Guid CustomerId { get; init; }
     public List<OrderItem> Items { get; init; }
}