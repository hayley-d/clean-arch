using GradTest.Contracts.Orders.Responses;
using GradTest.Domain.BoundedContexts.Orders.Entities;

namespace GradTest.Application.BoundedContexts.Orders.Mapping;

public static class OrderExtensions
{
    public static OrderResponse ToResponse(this Order order)
    {
        var responseItems = new List<OrderItemResponse>();
        
        foreach (var item in order.Items)
        {
            responseItems.Add(new OrderItemResponse
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
            });
        }
        
        return new OrderResponse
        {
            OrderId = order.Id,
            CustomerId = order.UserId,
            Items = responseItems,
        };
    }
}
