using GradTest.Domain.BoundedContexts.Orders.Entities;

namespace GradTest.Contracts.Orders.Requests;

public class CreateOrderRequest
{
    public List<OrderItemRequest> Items { get; init; }
}

public class OrderItemRequest
{
    public Guid ProductId { get; init; }
    public int Quantity { get; init; }
}