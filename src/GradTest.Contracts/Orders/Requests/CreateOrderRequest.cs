using GradTest.Domain.BoundedContexts.Orders.Entities;

namespace GradTest.Contracts.Orders.Requests;

public class CreateOrderRequest
{
    public List<OrderItem> Items { get; init; }
}