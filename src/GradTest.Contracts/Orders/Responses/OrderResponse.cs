using GradTest.Domain.BoundedContexts.Orders.Entities;

namespace GradTest.Contracts.Orders.Responses;

public class OrderResponse
{
     public Guid OrderId { get; init; }
     public Guid CustomerId { get; init; }
     public List<OrderItemResponse> Items { get; init; }
     
     public decimal ZarExchangeRate { get; init; }
}

public class OrderItemResponse
{
     public Guid ProductId { get; init; }
     public int Quantity { get; init; }
}